using Arcation.API.Extentions;
using Arcation.Core;
using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CompanyLocationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // TODO: ERROR IN ADD AND UPDATE METHODS:
        public CompanyLocationsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // api/CompanyLocations => Get All Location Related to Specific Company:
        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetAll([FromRoute] int? companyId)
        {
            if (companyId != null)
            {
                var entities = await _unitOfWork.Locations.GetAllLocationsAsync(HttpContext.GetBusinessId(), companyId);
                if(entities != null)
                {
                    return Ok(_mapper.Map<IEnumerable<LocationViewModel>>(entities));
                }
                return NoContent();
            }
            return NotFound();
        }

        // api/CompanyLocations/{companyId}/Location/{id} => Get Single Location Related to Specific Company:
        [HttpGet("{companyId}/Location/{id}", Name = "GetLocation")]
        public async Task<IActionResult> GetLocation([FromRoute] int? companyId, [FromRoute] int? id)
        {
            if (companyId != null && id != null)
            {
                var entity = await _unitOfWork.Locations.GetLocation(HttpContext.GetBusinessId(), id);
                if (entity != null)
                {
                    return Ok(_mapper.Map<LocationViewModel>(entity));
                }
                return NoContent();
            }
            return NotFound();
        }

        // api/CompanyLocations/{companyId} => All Location And Fill BandLocations Many To Many RelationShip:
        [HttpPost("{companyId}")]
        public async Task<IActionResult> AddLocation([FromRoute] int? companyId, [FromBody] AddLocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(companyId != null)
                {
                    var isExist = await _unitOfWork.Locations.FindAsync(l => l.LocationName == model.LocationName && l.BusinessId == HttpContext.GetBusinessId() && l.CompanyId == companyId);
                    if (isExist == null)
                    {

                        Location location = _mapper.Map<Location>(model);
                        location.IsDeleted = false;
                        location.CreatedAt = DateTime.UtcNow;
                        location.CreatedBy = HttpContext.GetUserId();
                        location.BusinessId = HttpContext.GetBusinessId();

                        await _unitOfWork.Locations.AddAsync(location);
                        await _unitOfWork.Complete();

                        var bandLocations = new List<BandLocation>();

                        foreach (int id in model.bandIds)
                        {
                            var bandLocation = new BandLocation
                            {
                                BandId = id,
                                LocationId = location.Id,
                                CreatedAt = location.CreatedAt,
                                CreatedBy = location.CreatedBy,
                                BusinessId = location.BusinessId

                            };
                            bandLocations.Add(bandLocation);
                        }

                        await _unitOfWork.BandLocations.AddRangeAsync(bandLocations);
                        if (await _unitOfWork.Complete())
                        {
                            var addedLocation = await _unitOfWork.Locations.GetLocation(HttpContext.GetBusinessId(), location.Id);
                            return CreatedAtRoute("GetLocation", new { controller = "CompanyLocations", companyId = location.CompanyId, id = location.Id }, _mapper.Map<LocationViewModel>(addedLocation));
                        }
                        return BadRequest();


                    }
                    return BadRequest("exist");
                }
                return BadRequest();
            }
            return BadRequest(ModelState);
        }

        // api/CompanyLocations/{id} => Update Location And Update BandLocations :
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompnay([FromRoute] int? id, [FromBody] UpdateLocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    LocationViewModel locationViewModel = new();
                    Location queryLocation = await _unitOfWork.Locations.FindAsync(c => c.Id == id && c.BusinessId == HttpContext.GetBusinessId());


                    if (queryLocation != null)
                    {
                        queryLocation.LocationName = model.LocationName;
                        queryLocation.StartingDate = model.StartingDate;
                        queryLocation.EndingDate = model.EndingDate;

                        var oldlist = await _unitOfWork.BandLocations.GetBandsId(queryLocation.Id);

                        List<BandLocation> bandLocations = await UpdateBandLocation(model, queryLocation, oldlist);
                        if(bandLocations.Count > 0)
                        {
                            await _unitOfWork.BandLocations.AddRangeAsync(bandLocations);
                        }



                        await _unitOfWork.Complete();
                        Location tryMapper = await _unitOfWork.Locations.GetLocation(HttpContext.GetBusinessId(), id);
                        return Ok(_mapper.Map<LocationViewModel>(tryMapper));
                        
                       /* Location addedLocation = await _unitOfWork.Locations.FindAsync(e => e.Id == queryLocation.Id && e.BusinessId == HttpContext.GetBusinessId());
                        IEnumerable<BandLocation> queryBandLocation = await _unitOfWork.BandLocations.FindAllAsync(e => e.LocationId == addedLocation.Id && !e.IsDeleted && e.BusinessId == HttpContext.GetBusinessId(), new[] { "Band" });

                        locationViewModel.LocationName = addedLocation.LocationName;
                        locationViewModel.EndingDate = addedLocation.EndingDate;
                        locationViewModel.StartingDate = addedLocation.StartingDate;
                        locationViewModel.LocationState = addedLocation.LocationState;
                        locationViewModel.Id = addedLocation.Id;
                        List<BandLocationDto> bandLocationDtos = new List<BandLocationDto>();
                        foreach(BandLocation band in queryBandLocation)
                        {
                            BandLocationDto bandLocationDto = new BandLocationDto
                            {
                                BandId = band.BandId,
                                BandLocationId = band.Id,
                                BandName = band.Band.BandName
                            };
                            bandLocationDtos.Add(bandLocationDto);
                        }
                        locationViewModel.Bands = bandLocationDtos;

                        return Ok(locationViewModel);*/
                        
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }


        // ToDo Updated All related Tabels:
        // api/CompanyLocations/{id} => Delete Location:
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation([FromRoute] int? id)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    Location queryLocation = await _unitOfWork.Locations.FindAsync(c => c.Id == id);
                    if (queryLocation != null)
                    {
                        queryLocation.IsDeleted = true;
                        await _unitOfWork.Complete();
                        return NoContent();

                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }


        #region Helper:

        // Update BandLocation Tabel:
        private async Task<List<BandLocation>> UpdateBandLocation(UpdateLocationViewModel model, Location queryLocation, List<int> oldlist)
        {
            var newList = new List<int>();
            var deletedList = new List<int>();
            var bandLocations = new List<BandLocation>();

            for (int i = 0; i < model.bandIds.Count; i++)
            {
                if (!oldlist.Contains(model.bandIds[i]))
                {
                    newList.Add(model.bandIds[i]);
                }
            }


            for (int i = 0; i < oldlist.Count; i++)
            {
                if (!model.bandIds.Contains(oldlist[i]))
                {
                    deletedList.Add(oldlist[i]);
                }
            }

            if (newList.Count > 0)
            {
                foreach (int newid in newList)
                {
                    Band band = await _unitOfWork.Bands.FindAsync(e => e.Id == newid && e.BusinessId == HttpContext.GetBusinessId());
                    if(band != null)
                    {
                        var bandLocation = new BandLocation
                        {
                            BandId = newid,
                            LocationId = queryLocation.Id,
                            IsDeleted = false,
                            BusinessId = HttpContext.GetBusinessId(),
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = HttpContext.GetUserId()
                        };
                        bandLocations.Add(bandLocation);
                    }    
                }
            }

            if (deletedList.Count > 0)
            {
                foreach (int deletedId in deletedList)
                {
                    BandLocation queryBandLocation = await _unitOfWork.BandLocations.FindAsync(e => e.LocationId == queryLocation.Id && e.BandId == deletedId && !e.IsDeleted);
                    queryBandLocation.IsDeleted = true;
                    await _unitOfWork.Complete();
                }
            }

            return bandLocations;
        }

        #endregion

    }
}

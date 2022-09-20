using Arcation.API.Extentions;
using Arcation.Core;
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
    [Authorize(Roles ="Admin, User")]
    public class LeaderAppController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public LeaderAppController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // api/Leaderapp
        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            string businessId = HttpContext.GetBusinessId();
            string userId = HttpContext.GetUserId();
            Leader queryLeader = await _unitOfWork.Leaders.GetLeaderDetail(userId, businessId);

            if(queryLeader != null)
            {
                List<LeaderLocations> Dtos = new();

                List<int> locationIds =  _unitOfWork.BandLocationLeaders.GetLocationIdsForLeader(queryLeader.Id, businessId);                
                foreach(int locationId in locationIds)
                {
                    Location location = await _unitOfWork.Locations.FindAsync(e => e.Id == locationId && !e.IsDeleted);
                    if(location != null)
                    {
                        BandLocationLeader bandLocationLeader =  _unitOfWork.BandLocationLeaders.GetForLeader(queryLeader.Id, locationId);
                        List<int> bandIds =  _unitOfWork.BandLocationLeaders.GetBandIdsForLeaderLocation(locationId, queryLeader.Id);
                        List<LeaderLocationBands> leaderLocationBands = new List<LeaderLocationBands>();                        
                        foreach (int bandId in bandIds)
                        {
                            Console.WriteLine(bandId);
                            Band band = await _unitOfWork.Bands.FindAsync(e => e.Id == bandId && !e.IsDeleted);
                            if(band != null)
                            {
                                LeaderLocationBands LeaderLocationBand = new LeaderLocationBands
                                {
                                    BandLocationLeaderId = bandLocationLeader.Id,
                                    BandId = band.Id,
                                    BandName = band.BandName
                                };
                                leaderLocationBands.Add(LeaderLocationBand);
                            }
                        }

                        if(leaderLocationBands.Count > 0)
                        {
                            LeaderLocations leaderLocation = new LeaderLocations
                            {
                                LocationId = locationId,
                                LocationName = location.LocationName,
                                BandLocations = leaderLocationBands
                            };
                            Dtos.Add(leaderLocation);
                        }
                    }
                }
               /* var bandLocationLeaders = await _unitOfWork.BandLocationLeaders.GetLeaderApp(queryLeader.Id, queryLeader.BusinessId);
                var filterLocationIds = new List<int>();
                var list = new List<LeaderLocations>();*/

                // Get LocationIds: 
                /*foreach (BandLocationLeader bandLocationLeader in bandLocationLeaders)
                {
                    var locationIds = await _unitOfWork.BandLocations.GetLocationIds(bandLocationLeader.BandLocationId);
                    foreach (int locationId in locationIds)
                    {
                        if (!filterLocationIds.Contains(locationId))
                        {
                            filterLocationIds.Add(locationId);
                        }
                    }
                }
                
                foreach(int id in filterLocationIds)
                {
                    Location location = await _unitOfWork.Locations.LocationAsync(id);
                    if(location != null)
                    {

                        List<LeaderLocationBands> leaderLocationBands1 = new List<LeaderLocationBands>();
                        if(location.BandLocations.Count > 0)
                        {
                            foreach(BandLocation bandLocation in location.BandLocations)
                            {
                                BandLocationLeader bandLocationLeader = await _unitOfWork.BandLocationLeaders.FindAsync(e => e.BandLocationId == bandLocation.Id && e.LeaderId == queryLeader.Id);
                                LeaderLocationBands leaderLocationBands = new LeaderLocationBands
                                {
                                    BandLocationLeaderId = bandLocationLeader.Id,
                                    BandId = bandLocation.BandId,
                                    BandName = bandLocation.Band.BandName
                                };
                                leaderLocationBands1.Add(leaderLocationBands);
                            }
                            
                        }

                        LeaderLocations leaderLocations = new LeaderLocations
                        {
                            LocationId = location.Id,
                            LocationName = location.LocationName,
                            BandLocations = leaderLocationBands1
                        };
                        list.Add(leaderLocations);
                    }
                }*/

                return Ok(Dtos);

                /*var locations = "null";

                *//*var locations = await _unitOfWork.Locations.LocaionsWithBandsRelatedToLeader(HttpContext.GetUserId(), HttpContext.GetBusinessId());
                var list = new List<LeaderLocations>();
                var locationIds = queryLeader.BandLocationLeaders.Select(e => e.BandLocation.LocationId);

                foreach (int LocationId in locationIds)
                {
                    Location queryLocation = await _unitOfWork.Locations.FindAsync(e => e.Id == LocationId);
                    var bandIds = queryLeader.BandLocationLeaders.Where(e => e.BandLocation.LocationId == LocationId).Select(e => e.BandLocation.BandId);
                    List<LeaderLocationBands> leaderLocationBands = new();

                    foreach (int bandId in bandIds)
                    {

                        Band queryBand = await _unitOfWork.Bands.FindAsync(e => e.Id == bandId);
                        BandLocationLeader queryBandLocationLeader = queryLeader.BandLocationLeaders.FirstOrDefault(e => e.BandLocation.LocationId == LocationId && e.BandLocation.BandId == bandId);
                        LeaderLocationBands leaderLocationBands1 = new LeaderLocationBands
                        {
                            BandId = bandId,
                            BandName = queryBand.BandName,
                            BandLocationLeaderId = queryBandLocationLeader.Id
                        };
                        leaderLocationBands.Add(leaderLocationBands1);
                    }

                    LeaderLocations result = new LeaderLocations
                    {
                        LocationName = queryLocation.LocationName,
                        LocationId = LocationId,
                        BandLocations = leaderLocationBands

                    };
                    list.Add(result);
                }*//*
                return Ok(bandLocationLeaders.First());*/
            }
            return NotFound();
        }

        // api/Leaderapp/periods/{bandLocationLeaderId}
        [HttpGet("periods/{bandLocationLeaderId}")]
        public async Task<IActionResult> GetPeriods([FromRoute] int? bandLocationLeaderId)
        {
            if(bandLocationLeaderId != null)
            {
                var periods = await _unitOfWork.BandLocationLeaderPeriods.GetPeriods(bandLocationLeaderId, HttpContext.GetUserId(), HttpContext.GetBusinessId());
                if (periods != null)
                {
                    return Ok(_mapper.Map<IEnumerable<PeriodsLeaderApp>>(periods));
                }
                return NotFound();
            }
            return NotFound();
        }

    }
}

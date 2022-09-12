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
    [Authorize(Roles ="Admin")]
    public class BandsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // TODO: NEED TO RECODE THIS CLASS:

        public BandsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Main
        // api/bands :
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Band> entities = await _unitOfWork.Bands.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && !e.IsDeleted);
            if(entities != null)
            {
                return Ok(_mapper.Map<IEnumerable<BandViewModel>>(entities));
            }
            return NoContent();
        }

        // api/bands/{id}
        [HttpGet("{id}", Name = "GetBand")]
        public async Task<IActionResult> GetBand([FromRoute] int? id)
        {
            if(id != null)
            {
                Band entity = await _unitOfWork.Bands.FindAsync(c => c.Id == id && !c.IsDeleted && c.BusinessId == HttpContext.GetBusinessId());
                if (entity != null)
                {
                    return Ok(_mapper.Map<BandViewModel>(entity));
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/bands :
        [HttpPost]
        public async Task<IActionResult> CreateBand([FromBody] AddBandViewModel model)
        {
            if (ModelState.IsValid)
            {
                Band isNameExist = await _unitOfWork.Bands.FindAsync(b => b.BandName == model.BandName && b.BusinessId == HttpContext.GetBusinessId() && !b.IsDeleted);
                if(isNameExist == null)
                {
                    Band band = new();
                    band.BandName = model.BandName;
                    band.IsDeleted = false;
                    band.CreatedAt = DateTime.UtcNow;
                    band.CreatedBy = HttpContext.GetUserId();
                    band.BusinessId = HttpContext.GetBusinessId();

                    Band result = await _unitOfWork.Bands.AddAsync(band);
                    if (await _unitOfWork.Complete() && result != null)
                    {
                        return CreatedAtRoute("GetBand", new { controller = "Bands", id = result.Id }, _mapper.Map<BandViewModel>(result));
                    }
                    return BadRequest();
                }
                return BadRequest("هذا الاسم موجود بالفعل");
            }
            return BadRequest(ModelState);
        }

        // api/Bands/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBand([FromRoute] int? id, [FromBody] UpdateBandViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    Band isNameExist = await _unitOfWork.Bands.FindAsync(b => b.BandName == model.BandName);
                    if(isNameExist == null)
                    {
                        Band queryBand = await _unitOfWork.Bands.FindAsync(c => c.Id == id);
                        if (queryBand != null)
                        {
                            queryBand.BandName = model.BandName;
                            _unitOfWork.Bands.Update(queryBand);
                            await _unitOfWork.Complete();
                            return Ok(_mapper.Map<BandViewModel>(queryBand));

                        }
                        return NotFound();
                    }
                    return BadRequest("هذا الاسم موجود بالفعل");
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/Bands/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBand([FromRoute] int? id)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    Band queryBand = await _unitOfWork.Bands.FindAsync(c => c.Id == id);
                    if (queryBand != null)
                    {
                        queryBand.IsDeleted = true;
                        _unitOfWork.Bands.Update(queryBand);
                        await _unitOfWork.Complete();
                        return new NoContentResult();
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        #endregion


    }
}

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
    [Authorize(Roles = "Admin")]
    public class BLWestedsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BLWestedsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // api/BLWesteds/{bandLocationId} => Get All BLWested:
        [HttpGet("{bandLocationId}")]
        public async Task<IActionResult> GetAll([FromRoute] int? bandLocationId)
        {
            if (bandLocationId != null)
            {
                IEnumerable<BLWested> entities = await _unitOfWork.BLWesteds.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.IsDeleted == false && e.BandLocationId == bandLocationId);
                return Ok(_mapper.Map<IEnumerable<BLWestedDto>>(entities));
            }
            return NotFound();
        }

        // api/BLWesteds/{bandLocationId}/BLWested/{id} => Get Single BLWested:
        [HttpGet("{bandLocationId}/BLWested/{id}", Name = "GetBLWested")]
        public async Task<IActionResult> GetBLWested([FromRoute] int? bandLocationId, [FromRoute] int? id)
        {
            if (id != null)
            {
                BLWested entity = await _unitOfWork.BLWesteds.FindAsync(b => b.Id == id && b.IsDeleted == false && b.BusinessId == HttpContext.GetUserId() && b.BandLocationId == bandLocationId);
                if (entity != null)
                {
                    return Ok(_mapper.Map<BLWestedDto>(entity));
                }
                return NotFound();
            }
            return NotFound();

        }

        // api/BLWesteds : => add BLWested
        [HttpPost]
        public async Task<IActionResult> AddBLWested([FromBody] AddBLWestedDto dto)
        {
            if (ModelState.IsValid)
            {
                BLWested newBLWested = _mapper.Map<BLWested>(dto);
                newBLWested.IsDeleted = false;
                newBLWested.CreatedAt = DateTime.UtcNow;
                newBLWested.CreatedBy = HttpContext.GetUserId();
                newBLWested.BusinessId = HttpContext.GetBusinessId();

                await _unitOfWork.BLWesteds.AddAsync(newBLWested);
                if (await _unitOfWork.Complete())
                {
                    return CreatedAtRoute("GetBLWested", new { controller = "BLWesteds", id = newBLWested.Id, bandLocationId = newBLWested.BandLocationId }, _mapper.Map<BLWestedDto>(newBLWested));
                }
                return BadRequest();

            }
            return BadRequest(ModelState);
        }

        // api/BLWesteds/{id} => Update BLWested
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBLWested([FromRoute] int? id, [FromBody] UpdateBLWestedDto dto)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    BLWested queryBLWested = await _unitOfWork.BLWesteds.FindAsync(b => b.Id == id && b.BusinessId == HttpContext.GetUserId());
                    if (queryBLWested != null)
                    {
                        queryBLWested.Estatement = dto.Estatement;
                        queryBLWested.Date = dto.Date;
                        queryBLWested.Note = dto.Note;
                        queryBLWested.Price = dto.Price;
                        await _unitOfWork.Complete();
                        return Ok(_mapper.Map<BLWestedDto>(queryBLWested));
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/BLWesteds/{id} => Delete BLWested
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBLWested([FromRoute] int? id)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    BLWested queryBLWested = await _unitOfWork.BLWesteds.FindAsync(b => b.Id == id);
                    if (queryBLWested != null)
                    {
                        queryBLWested.IsDeleted = true;
                        if(await _unitOfWork.Complete())
                        {
                            return NoContent();
                        }
                        return BadRequest();  
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }


    }
}

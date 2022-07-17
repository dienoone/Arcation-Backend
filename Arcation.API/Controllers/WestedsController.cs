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
    [Authorize(Roles = "Admin, User")]
    public class WestedsController : ControllerBase
    {
        // BandLocationLeaderPeriodId
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public WestedsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // api/westeds/{bandLocationLeaderPeriodId} => Get All BLWested:
        [HttpGet("{bandLocationLeaderPeriodId}")]
        public async Task<IActionResult> GetAll([FromRoute] int? bandLocationLeaderPeriodId)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                IEnumerable<Wested> entities = await _unitOfWork.LeaderWesteds.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && !e.IsDeleted && e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId);
                if (entities != null)
                {
                    return Ok(_mapper.Map<IEnumerable<LeaderWestedDto>>(entities));
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/westeds/{bandLocationLeaderPeriodId}/wested/{Id} => Get Single Wested:
        [HttpGet("{bandLocationLeaderPeriodId}/wested/{Id}", Name = "GetWested")]
        public async Task<IActionResult> GetWested([FromRoute] int? bandLocationLeaderPeriodId, [FromRoute] int? Id)
        {
            if (Id != null && bandLocationLeaderPeriodId != null)
            {
                Wested entity = await _unitOfWork.LeaderWesteds.FindAsync(b => b.Id == Id && b.BusinessId == HttpContext.GetUserId() && b.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId);
                if (entity != null)
                {
                    return Ok(_mapper.Map<LeaderWestedDto>(entity));
                }
                return NotFound();
            }
            return NotFound();

        }

        // api/westeds : => add Wested
        [HttpPost]
        public async Task<IActionResult> CreateWested([FromBody] AddLeaderWestedDto dto)
        {
            if (ModelState.IsValid)
            {
                Wested newWested = _mapper.Map<Wested>(dto);
                newWested.IsDeleted = false;
                newWested.CreatedAt = DateTime.UtcNow;
                newWested.CreatedBy = HttpContext.GetUserId();
                newWested.BusinessId = HttpContext.GetBusinessId();

                var result = await _unitOfWork.LeaderWesteds.AddAsync(newWested);

                if (result != null && await _unitOfWork.Complete())
                {
                    return CreatedAtRoute("GetWested", new { controller = "Westeds", bandLocationLeaderPeriodId = newWested.BandLocationLeaderPeriodId, id = newWested.Id }, _mapper.Map<LeaderWestedDto>(newWested));
                }
                return BadRequest();

            }
            return BadRequest(ModelState);
        }

        // api/westeds/{Id} => Update Wested
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateTransiction([FromRoute] int? Id, [FromBody] UpdateLeaderWestedDto dto)
        {
            if (ModelState.IsValid)
            {
                if (Id != null)
                {
                    Wested queryWested = await _unitOfWork.LeaderWesteds.FindAsync(b => b.Id == Id && b.BusinessId == HttpContext.GetBusinessId());
                    if (queryWested != null)
                    {
                        queryWested.Estatement = dto.Estatement;
                        queryWested.Value = dto.Value;
                        queryWested.Date = dto.Date;
                        queryWested.Note = dto.Note;

                        if (await _unitOfWork.Complete())
                        {
                            return Ok(_mapper.Map<LeaderWestedDto>(queryWested));
                        }
                        return BadRequest();
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/westeds/{Id} => Delete Transiction
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteTransiction([FromRoute] int? Id)
        {
            if (Id != null)
            {
                Wested queryWested = await _unitOfWork.LeaderWesteds.FindAsync(t => t.Id == Id && t.BusinessId == HttpContext.GetBusinessId());
                if (queryWested != null)
                {
                    queryWested.IsDeleted = true;
                    var result = _unitOfWork.LeaderWesteds.Update(queryWested);
                    if (result != null && await _unitOfWork.Complete())
                    {
                        return new NoContentResult();
                    }
                    return BadRequest();
                }
                return NotFound();
            }
            return NotFound();
        }
    }
}

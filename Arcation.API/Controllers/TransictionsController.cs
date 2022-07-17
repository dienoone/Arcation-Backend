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
    public class TransictionsController : ControllerBase
    {
        // BandLocationLeaderPeriodId
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TransictionsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // api/transictions/{bandLocationLeaderPeriodId} => Get All Transictions:
        [HttpGet("{bandLocationLeaderPeriodId}")]
        public async Task<IActionResult> GetAll([FromRoute] int? bandLocationLeaderPeriodId)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                IEnumerable<Transiction> entities = await _unitOfWork.LeaderTransactions.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.IsDeleted == false && e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId);
                if(entities != null)
                {
                    return Ok(_mapper.Map<IEnumerable<TransictionDto>>(entities));
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/transictions/{bandLocationLeaderPeriodId}/transiction/{Id} => Get Single Transiction:
        [HttpGet("{bandLocationLeaderPeriodId}/transiction/{Id}", Name = "GetTransiction")]
        public async Task<IActionResult> GetTransiction([FromRoute] int? bandLocationLeaderPeriodId, [FromRoute] int? Id)
        {
            if (Id != null && bandLocationLeaderPeriodId != null)
            {
                Transiction entity = await _unitOfWork.LeaderTransactions.FindAsync(b => b.Id == Id && b.BusinessId == HttpContext.GetUserId() && b.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId);
                if (entity != null)
                {
                    return Ok(_mapper.Map<TransictionDto>(entity));
                }
                return NotFound();
            }
            return NotFound();

        }

        // api/transictions : => add Transiction
        [HttpPost]
        public async Task<IActionResult> CreateTransiction([FromBody] AddTransictionDto dto)
        {
            if (ModelState.IsValid)
            {
                Transiction newTransiction = _mapper.Map<Transiction>(dto);
                newTransiction.IsDeleted = false;
                newTransiction.CreatedAt = DateTime.UtcNow;
                newTransiction.CreatedBy = HttpContext.GetUserId();
                newTransiction.BusinessId = HttpContext.GetBusinessId();

                var result = await _unitOfWork.LeaderTransactions.AddAsync(newTransiction);

                if (result != null && await _unitOfWork.Complete())
                {
                    return CreatedAtRoute("GetTransiction", new { controller = "Transictions", bandLocationLeaderPeriodId = newTransiction.BandLocationLeaderPeriodId, id = newTransiction.Id }, _mapper.Map<TransictionDto>(newTransiction));
                }
                return BadRequest();
                
            }
            return BadRequest(ModelState);
        }

        // api/transictions/{Id} => Update Transiction
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateTransiction([FromRoute] int? Id, [FromBody] UpdateTransictionDto dto)
        {
            if (ModelState.IsValid)
            {
                if (Id != null)
                {
                    Transiction queryTransiction = await _unitOfWork.LeaderTransactions.FindAsync(b => b.Id == Id && b.BusinessId == HttpContext.GetBusinessId());
                    if (queryTransiction != null)
                    {
                        queryTransiction.Estatement = dto.Estatement;
                        queryTransiction.Value = dto.Value;
                        queryTransiction.Date = dto.Date;
                        queryTransiction.Note = dto.Note;

                        if (await _unitOfWork.Complete())
                        {
                            return Ok(_mapper.Map<TransictionDto>(queryTransiction));
                        }
                        return BadRequest();
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/transictions/{Id} => Delete Transiction
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteTransiction([FromRoute] int? Id)
        {
            if (Id != null)
            {
                Transiction queryTransiction = await _unitOfWork.LeaderTransactions.FindAsync(t => t.Id == Id && t.BusinessId == HttpContext.GetBusinessId());
                if (queryTransiction != null)
                {
                    queryTransiction.IsDeleted = true;
                    var result = _unitOfWork.LeaderTransactions.Update(queryTransiction);
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

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
    public class IncomesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IncomesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // api/Incomes/{bandLocationId} => Get All Incomes:
        [HttpGet("{bandLocationId}")]
        public async Task<IActionResult> GetAll([FromRoute] int? bandLocationId)
        {
            if (bandLocationId != null)
            {
                IEnumerable<Income> entities = await _unitOfWork.Incomes.FindAllAsync(i => i.BusinessId == HttpContext.GetBusinessId() && i.IsDeleted == false && i.BandLocationId == bandLocationId);
                return Ok(_mapper.Map<IEnumerable<IncomeDto>>(entities));
            }
            return NotFound();
        }

        // api/Incomes/{bandLocationId}/Income/{id} => Get Single Income:
        [HttpGet("{bandLocationId}/Income/{id}", Name = "GetIncome")]
        public async Task<IActionResult> GetIncome([FromRoute] int? bandLocationId, [FromRoute] int? id)
        {
            if (id != null)
            {
                Income entity = await _unitOfWork.Incomes.FindAsync(i => i.IncomeId == id && i.IsDeleted == false && i.BusinessId == HttpContext.GetBusinessId() && i.BandLocationId == bandLocationId);
                if (entity != null)
                {
                    return Ok(_mapper.Map<IncomeDto>(entity));
                }
                return NotFound();
            }
            return NotFound();

        }

        // api/Incomes : => add Income
        [HttpPost]
        public async Task<IActionResult> AddIncome([FromBody] AddIncomeDto dto)
        {
            if (ModelState.IsValid)
            {
                Income newIncome = _mapper.Map<Income>(dto);
                newIncome.IsDeleted = false;
                newIncome.CreatedBy = HttpContext.GetUserId();
                newIncome.CreatedAt = DateTime.UtcNow;
                newIncome.BusinessId = HttpContext.GetBusinessId();

                await _unitOfWork.Incomes.AddAsync(newIncome);
                if (await _unitOfWork.Complete())
                {
                    return CreatedAtRoute("GetIncome", new { controller = "Incomes", id = newIncome.IncomeId, bandLocationId = newIncome.BandLocationId }, _mapper.Map<IncomeDto>(newIncome));
                }
                return BadRequest();

            }
            return BadRequest(ModelState);
        }

        // api/Incomes/{id} => Update Income
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncome([FromRoute] int? id, [FromBody] UpdateIncomeDto dto)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    Income queryIncome = await _unitOfWork.Incomes.FindAsync(i => i.IncomeId == id && i.BusinessId == HttpContext.GetBusinessId());
                    if (queryIncome != null)
                    {
                        queryIncome.Estatement = dto.Estatement;
                        queryIncome.Note = dto.Note;
                        queryIncome.Price = dto.Price;
                        queryIncome.Date = dto.Date;
                        await _unitOfWork.Complete();
                        return Ok(_mapper.Map<IncomeDto>(queryIncome));
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/Incomes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome([FromRoute] int? id)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    Income queryIncome = await _unitOfWork.Incomes.FindAsync(i => i.IncomeId == id);
                    if (queryIncome != null)
                    {

                        queryIncome.IsDeleted = true;
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

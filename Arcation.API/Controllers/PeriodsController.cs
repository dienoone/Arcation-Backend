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
    public class PeriodsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        // Periods Related to BandLocation:
        public PeriodsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #region TODO:

        // api/periods/{bandLocationId}
        [HttpGet("{bandLocationId}")]
        public async Task<IActionResult> GetAll([FromRoute] int? bandLocationId)
        {
            IEnumerable<Period> entities = await _unitOfWork.Periods.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && !e.IsDeleted);
            if (entities != null)
            {
                return Ok(_mapper.Map<IEnumerable<AllPeriodDto>>(entities));
            }
            return NoContent();
        }

        // api/periods/single/{id} => Get Single Period: Need Filter for Employeees:
        [HttpGet("Single/{id}", Name = "GetPeriod")]
        public async Task<IActionResult> GetPeriod([FromRoute] int? id)
        {
            if (id != null)
            {
                Period queryPeriod = await _unitOfWork.Periods.GetPeriodDetail(id, HttpContext.GetBusinessId());
                if (queryPeriod != null)
                {
                    GlobalSinglePeriod globalSinglePeriod = _mapper.Map<GlobalSinglePeriod>(queryPeriod);
                    PeriodDetailDto periodDetailDto = _mapper.Map<PeriodDetailDto>(queryPeriod);
                    periodDetailDto.RemainderForEmployees = periodDetailDto.TotalSalaryOfEmployees - periodDetailDto.TotalPaied;
                    globalSinglePeriod.PeriodDetail = periodDetailDto;
                    List<PeriodEmployees> periodEmployees = new();
                    foreach(var period in queryPeriod.BandLocationLeaderPeriods)
                    {
                        foreach(var employee in period.BandLocationLeaderPeriodEmployees)
                        {
                            PeriodEmployees employees = new PeriodEmployees
                            {
                                EmployeeId = employee.Employee.Id,
                                EmployeeName = employee.Employee.Name
                            };
                            if (!periodEmployees.Contains(employees))
                            {
                                periodEmployees.Add(employees);
                            }
                        }
                    }
                    globalSinglePeriod.PeriodEmployees = periodEmployees;

                    return Ok(globalSinglePeriod);
                }
                return NotFound();
            }
            return NotFound();
        }

        #endregion

        // api/periods/{BandLocationId} => create period
        [HttpPost("{BandLocationId}")]
        public async Task<IActionResult> CreatePeriod([FromRoute] int? BandLocationId, [FromBody] AddPeriodDto dto)
        {
            if (BandLocationId != null)
            {
                if (ModelState.IsValid)
                {
                    Period isExist = await _unitOfWork.Periods.FindAsync(e => e.Name == dto.Name && e.BusinessId == HttpContext.GetBusinessId());
                    if (isExist == null)
                    {
                        Period newPeriod = new Period
                        {
                            Name = dto.Name,
                            BandLocationId = (int)BandLocationId,
                            StartingDate = DateTime.UtcNow,
                            EndingDate = null,
                            State = true,
                            IsDeleted = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = HttpContext.GetUserId(),
                            BusinessId = HttpContext.GetBusinessId()
                        };
                        var result = await _unitOfWork.Periods.AddAsync(newPeriod);

                        if (result != null)
                        {
                            if (await _unitOfWork.Complete())
                            {
                                Period queryPeriod = await _unitOfWork.Periods.GetPeriodDetail(result.Id, HttpContext.GetBusinessId());
                                if (queryPeriod != null)
                                {
                                    GlobalSinglePeriod globalSinglePeriod = _mapper.Map<GlobalSinglePeriod>(queryPeriod);
                                    PeriodDetailDto periodDetailDto = _mapper.Map<PeriodDetailDto>(queryPeriod);
                                    periodDetailDto.RemainderForEmployees = periodDetailDto.TotalSalaryOfEmployees - periodDetailDto.TotalPaied;
                                    globalSinglePeriod.PeriodDetail = periodDetailDto;
                                    return CreatedAtRoute("GetPeriod", new { controller = "Periods", id = result.Id }, globalSinglePeriod);
                                }
                                return NotFound();                                
                            }
                            return BadRequest();
                        }
                        return BadRequest();

                    }
                    return BadRequest("هذا الاسم موجود بالفعل");
                }
                return BadRequest(ModelState);
            }
            return NotFound();
        }

        // api/periods/{BandLocationId} => Delete period
        [HttpDelete("{PeriodId}")]
        public async Task<IActionResult> DeletePeriod([FromRoute] int? PeriodId)
        {
            if (PeriodId != null)
            {
                var queryPeriod = await _unitOfWork.Periods.FindAsync(e => e.Id == PeriodId && e.BusinessId == HttpContext.GetBusinessId());
                if (queryPeriod != null)
                {
                    queryPeriod.IsDeleted = true;
                    if (await _unitOfWork.Complete())
                    {
                        return NoContent();
                    }
                    return BadRequest();
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/periods/{BandLocationId} => Update period
        [HttpPut("{PeriodId}")]
        public async Task<IActionResult> EditPeriod([FromRoute] int? PeriodId, [FromBody] UpdatePeriodDto dto)
        {
            if (PeriodId != null)
            {
                if (ModelState.IsValid)
                {
                    var queryPeriod = await _unitOfWork.Periods.FindAsync(e => e.Id == PeriodId && e.BusinessId == HttpContext.GetBusinessId());
                    if (queryPeriod != null)
                    {
                        queryPeriod.Name = dto.PeriodName;
                        queryPeriod.StartingDate = dto.StartingDate;
                        queryPeriod.EndingDate = dto.EndingDate;
                        queryPeriod.State = dto.PeriodState;

                        await _unitOfWork.Complete();

                        Period Period = await _unitOfWork.Periods.GetPeriodDetail(queryPeriod.Id, HttpContext.GetBusinessId());
                        if (Period != null)
                        {
                            GlobalSinglePeriod globalSinglePeriod = _mapper.Map<GlobalSinglePeriod>(Period);
                            PeriodDetailDto periodDetailDto = _mapper.Map<PeriodDetailDto>(Period);
                            periodDetailDto.RemainderForEmployees = periodDetailDto.TotalSalaryOfEmployees - periodDetailDto.TotalPaied;
                            globalSinglePeriod.PeriodDetail = periodDetailDto;
                            List<PeriodEmployees> periodEmployees = new();
                            foreach (var p in Period.BandLocationLeaderPeriods)
                            {
                                foreach (var employee in p.BandLocationLeaderPeriodEmployees)
                                {
                                    PeriodEmployees employees = new PeriodEmployees
                                    {
                                        EmployeeId = employee.Employee.Id,
                                        EmployeeName = employee.Employee.Name
                                    };
                                    if (!periodEmployees.Contains(employees))
                                    {
                                        periodEmployees.Add(employees);
                                    }
                                }
                            }
                            globalSinglePeriod.PeriodEmployees = periodEmployees;

                            return Ok(globalSinglePeriod);
                        }
                    }
                    return NotFound();
                }
                return BadRequest(ModelState);
            }
            return NotFound();
        }

    }
}

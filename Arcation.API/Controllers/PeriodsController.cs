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
            IEnumerable<Period> entities = await _unitOfWork.Periods.FindAllAsync(e => e.BandLocationId == bandLocationId && e.BusinessId == HttpContext.GetBusinessId() && !e.IsDeleted);
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
                string businessId = HttpContext.GetBusinessId();
                Period queryPeriod = await _unitOfWork.Periods.GetPeriodIncludeBandLocation(id, businessId);
                if (queryPeriod != null)
                {
                    GlobalSinglePeriod globalSinglePeriod = new();

                    PeriodDetailDto periodDetailDto = new();
                    periodDetailDto.TotalWesteds = _unitOfWork.LeaderWesteds.GetPeriodReport(id, businessId);
                    periodDetailDto.TotalTransictions = _unitOfWork.LeaderTransactions.GetPeriodReport(id, businessId);
                    periodDetailDto.TotalDays = _unitOfWork.Attendances.GetPeriodDaysReport(id, businessId);
                    periodDetailDto.TotalPaied = _unitOfWork.Attendances.GetTotalPaiedOfEmployeePeriodReoprt(id, businessId) + _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetTotalPaiedOfEmployeePeriodReoprt(id, businessId);
                    periodDetailDto.TotalSalaryOfEmployees = _unitOfWork.Attendances.GetTotalSalaryOfEmployeePeriodReoprt(id, businessId) + _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetTotalSalaryOfEmployeePeriodReoprt(id, businessId);
                    periodDetailDto.CountOfEmployees = _unitOfWork.BandLocationLeaderPeriodEmployees.GetPeriodCountEmployees(id, businessId);
                    periodDetailDto.CountOfLeaders = _unitOfWork.BandLocationLeaderPeriods.GetPeriodConutOfLeadersReport(id, businessId);
                    periodDetailDto.PeriodName = queryPeriod.Name;
                    periodDetailDto.StartingDate = queryPeriod.StartingDate;
                    periodDetailDto.EndingDate = queryPeriod.EndingDate;
                    periodDetailDto.PeriodState = queryPeriod.State;
                    periodDetailDto.PeriodId = queryPeriod.Id;
                    periodDetailDto.CountOfLeaders = _unitOfWork.BandLocationLeaderPeriods.GetPeriodConutOfLeadersReport(id, businessId);
                    periodDetailDto.BandName = queryPeriod.BandLocation.Band.BandName;
                    periodDetailDto.LocationName = queryPeriod.BandLocation.Location.LocationName;
                    periodDetailDto.RemainderForEmployees = periodDetailDto.TotalSalaryOfEmployees - periodDetailDto.TotalPaied;
                    globalSinglePeriod.PeriodDetail = periodDetailDto;


                    List<PeriodEmployees> periodEmployees = new();
                    var queryPeriodEmployees = await _unitOfWork.BandLocationLeaderPeriodEmployees.GetLeaderPeriodsAsync(id, businessId);
                    foreach (var period in queryPeriodEmployees)
                    {
                        PeriodEmployees employees = new PeriodEmployees
                        {
                            EmployeeId = period.Employee.Id,
                            EmployeeName = period.Employee.Name
                        };
                        if (!periodEmployees.Contains(employees))
                        {
                            periodEmployees.Add(employees);
                        }
                    }

                    globalSinglePeriod.PeriodEmployees = periodEmployees;

                    List<PeriodLeaders> periodLeaders = new();
                    var leaders = await _unitOfWork.BandLocationLeaderPeriods.GetPeriodsAsync(id, businessId);
                    foreach (var leader in leaders)
                    {
                        PeriodLeaders employees = new PeriodLeaders
                        {
                            BandLocationLeaderPeriodId = leader.Id,
                            LeaderName = leader.BandLocationLeader.Leader.Name,
                            LeaderID = leader.BandLocationLeader.LeaderId,
                            LeaderState = leader.State
                        };
                        if (!periodLeaders.Contains(employees))
                        {
                            periodLeaders.Add(employees);
                        }
                    }
                    globalSinglePeriod.PeriodLeaders = periodLeaders;

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
                    Period isExist = await _unitOfWork.Periods.FindAsync(e => e.Name == dto.Name && e.BusinessId == HttpContext.GetBusinessId() && e.BandLocationId == BandLocationId && !e.IsDeleted);
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

                                return CreatedAtRoute("GetPeriod", new { controller = "Periods", id = result.Id }, "Added");
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

                        return Ok();
                    }
                    return NotFound();
                }
                return BadRequest(ModelState);
            }
            return NotFound();
        }


        


    }
}

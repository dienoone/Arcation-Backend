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
    public class LeaderEmployeesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        // bandLocationLeaderPeriodId:
        // employees With Periods Related to BandLocationLeaderPeriod:

        public LeaderEmployeesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // api/leaderEmployees/{bandLocationLeaderPeriodId}
        [HttpGet("{bandLocationLeaderPeriodId}")]
        public async Task<IActionResult> GetAllEmployeesWithPeriods([FromRoute] int? bandLocationLeaderPeriodId)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                var entities = await _unitOfWork.BandLocationLeaderPeriodEmployees.GetPeriodEmployeeDataAsync(bandLocationLeaderPeriodId, HttpContext.GetBusinessId());
                return Ok(_mapper.Map<IEnumerable<BandLocationLeaderPeriodEmployeeDto>>(entities));
            }
            return NotFound();
        }

        // NotNeeded :
        // api/leaderEmployees/{bandLocationLeaderPeriodId}/EmployeePeriods/{Id}
        [HttpGet("{bandLocationLeaderPeriodId}/EmployeePeriods/{Id}")]
        public async Task<IActionResult> GetEmployeeWithPeriod([FromRoute] int? bandLocationLeaderPeriodId, [FromRoute] int? Id)
        {
            if (bandLocationLeaderPeriodId != null && Id != null)
            {
                var entity = await _unitOfWork.BandLocationLeaderPeriodEmployees.GetSinglePeriodEmployeeDataAsync(bandLocationLeaderPeriodId, Id, HttpContext.GetBusinessId());
                if (entity != null)
                {
                    return Ok(_mapper.Map<BandLocationLeaderPeriodEmployeeDto>(entity));
                }
                return NotFound();
            }
            return BadRequest();
        }

        // api/leaderEmployees/updateMainPeriod/{bandLocationLeaderPeriodEmployeeId}
        [HttpPut("UpdateMainPeriod/{bandLocationLeaderPeriodEmployeeId}")]
        public async Task<IActionResult> UpdateMainPeriod([FromRoute] int? bandLocationLeaderPeriodEmployeeId, [FromBody] UpdateMainPeriodForEmployee dto)
        {
            if(ModelState.IsValid && bandLocationLeaderPeriodEmployeeId != null)
            {
                BandLocationLeaderPeriodEmployee entity = await _unitOfWork.BandLocationLeaderPeriodEmployees.FindAsync(e => e.Id == bandLocationLeaderPeriodEmployeeId);
                if(entity != null)
                {
                    entity.State = dto.EmployeeState;
                    if (dto.EmployeeState)
                    {
                        entity.EndingDate = null;
                    }
                    else 
                    {
                        entity.EndingDate = DateTime.UtcNow;
                    }
                    await _unitOfWork.Complete();
                    var result = await _unitOfWork.BandLocationLeaderPeriodEmployees.GetSinglePeriodEmployeeDataAsync(entity.BandLocationLeaderPeriodId, entity.Id, HttpContext.GetBusinessId());
                    if (result != null)
                    {
                        return Ok(_mapper.Map<BandLocationLeaderPeriodEmployeeDto>(entity));
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest();
        }
          
        // AssignEmployeesToBandLocationLeaderPeriod:
        // api/leaderEmployees/assignEmployeesToLeaderPeriod
        [HttpPost("AssignEmployeesToLeaderPeriod")]
        public async Task<IActionResult> AssignEmployeesToBandLocationLeaderPeriod([FromBody] AssignEmployeesToBandLocationLeaderPeriodDto dto)
        {
            if (ModelState.IsValid)
            {
                List<BandLocationLeaderPeriodEmployee> bandLocationLeaderPeriodEmployees = new List<BandLocationLeaderPeriodEmployee>();
                if (dto.EmployeeIds != null)
                {
                    foreach (int employeeId in dto.EmployeeIds)
                    {
                        BandLocationLeaderPeriodEmployee isExist = await _unitOfWork.BandLocationLeaderPeriodEmployees.FindAsync(bllpe => bllpe.EmployeeId == employeeId && bllpe.BandLocationLeaderPeriodId == dto.BandLocationLeaderPeriodId);
                        if (isExist == null)
                        {
                            Employee employee = await _unitOfWork.Employees.FindAsync(e => e.Id == employeeId);
                            if (employee != null)
                            {
                                BandLocationLeaderPeriodEmployee bandLocationLeaderPeriodEmployee = new BandLocationLeaderPeriodEmployee
                                {
                                    BandLocationLeaderPeriodId = dto.BandLocationLeaderPeriodId,
                                    EmployeeId = employeeId,
                                    IsDeleted = false,
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedBy = HttpContext.GetUserId(),
                                    BusinessId = HttpContext.GetBusinessId(),
                                    StartingDate = DateTime.UtcNow,
                                    EndingDate = null,
                                    State = true,
                                    EmployeeSalay = employee.Salary
                                };
                                bandLocationLeaderPeriodEmployees.Add(bandLocationLeaderPeriodEmployee);
                            }
                        }
                        else 
                        {
                            isExist.State = true;
                        }
                    }
                    
                    if (bandLocationLeaderPeriodEmployees.Count > 0)
                    {
                        var result = await _unitOfWork.BandLocationLeaderPeriodEmployees.AddRangeAsync(bandLocationLeaderPeriodEmployees);

                        if (result != null && await _unitOfWork.Complete())
                        {
                            return Ok("Added Successfully");
                        }
                        return BadRequest("حدث خطأ اثناء الاضافه");
                    }
                    return NoContent();
                }
                return BadRequest();
            }
            return BadRequest();
        }

        // api/leaderEmployees/deleteEmployee/{bandLocationLeaderPeriodEmployeeId}
        [HttpDelete("DeleteEmployee/{bandLocationLeaderPeriodEmployeeId}")]
        public async Task<IActionResult> DeleteBandLocationLeaderPeriodEmployee(int? bandLocationLeaderPeriodEmployeeId) 
        {
            if (bandLocationLeaderPeriodEmployeeId != null) 
            {
                var employee = await _unitOfWork.BandLocationLeaderPeriodEmployees.FindAsync(e => e.Id == bandLocationLeaderPeriodEmployeeId && e.BusinessId == HttpContext.GetBusinessId());
                if (employee != null) 
                {
                    employee.IsDeleted = true;
                    await _unitOfWork.Complete();
                    return NoContent();
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/leaderEmployees/subPeriod/{bandLocationLeaderPeriodEmployeePeriodId}
        [HttpGet("subPeriod/{bandLocationLeaderPeriodEmployeePeriodId}")]
        public async Task<IActionResult> GetSubPeriod([FromRoute]int? bandLocationLeaderPeriodEmployeePeriodId)
        {
            if(bandLocationLeaderPeriodEmployeePeriodId != null)
            {
                BandLocationLeaderPeriodEmployeePeriod subPeriod = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetSubPeriodIncludeAttendace(bandLocationLeaderPeriodEmployeePeriodId, HttpContext.GetBusinessId());
                if (subPeriod != null)
                {   
                    SubPeriodDetailDto subPeriodDetailDto = _mapper.Map<SubPeriodDetailDto>(subPeriod);
                    subPeriodDetailDto.Remainder = subPeriodDetailDto.TotalSalary - subPeriodDetailDto.TotalPaied - subPeriodDetailDto.TotalBorrow;
                    return Ok(subPeriodDetailDto);
                }
                return NotFound();
            }
            return NotFound();
        }

        // Leader Work
        // api/leaderEmployees/finishSubPeriod/{bandLocationLeaderPeriodEmployeePeriodId}
        [HttpPost("finishSubPeriod/{bandLocationLeaderPeriodEmployeePeriodId}")]
        public async Task<IActionResult> FinishSubPeriod([FromRoute] int? bandLocationLeaderPeriodEmployeePeriodId, [FromBody] FinishSupPeriodLeaderDto dto)
        {
            if (ModelState.IsValid) 
            {
                if (bandLocationLeaderPeriodEmployeePeriodId != null)
                {
                    BandLocationLeaderPeriodEmployeePeriod bandLocationLeaderPeriodEmployeePeriod = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.FindAsync(e => e.Id == bandLocationLeaderPeriodEmployeePeriodId);
                    if (bandLocationLeaderPeriodEmployeePeriod != null)
                    {
                        bandLocationLeaderPeriodEmployeePeriod.State = dto.EmployeeState;
                        await _unitOfWork.Complete();

                        BandLocationLeaderPeriodEmployeePeriod subPeriod = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetSubPeriodIncludeAttendace(bandLocationLeaderPeriodEmployeePeriodId, HttpContext.GetBusinessId());
                        if (subPeriod != null)
                        {
                            SubPeriodDetailDto subPeriodDetailDto = _mapper.Map<SubPeriodDetailDto>(subPeriod);
                            subPeriodDetailDto.Remainder = subPeriodDetailDto.TotalSalary - subPeriodDetailDto.TotalPaied - subPeriodDetailDto.TotalBorrow;
                            return Ok(subPeriodDetailDto);
                        }
                        return NotFound();

                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/leaderEmployees/finishSubPeriod/{bandLocationLeaderPeriodEmployeePeriodId}
        [HttpPut("updateSubPeriod/{bandLocationLeaderPeriodEmployeePeriodId}")]
        public async Task<IActionResult> UpdateSubPeriod([FromRoute] int? bandLocationLeaderPeriodEmployeePeriodId, [FromBody] UpdateSubPeriod dto)
        {
            if(ModelState.IsValid && bandLocationLeaderPeriodEmployeePeriodId != null)
            {
                BandLocationLeaderPeriodEmployeePeriod entity = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.FindAsync(e => e.Id == bandLocationLeaderPeriodEmployeePeriodId);
                if(entity != null)
                {
                    entity.EmployeeSalary = dto.EmployeeSalary;
                    await _unitOfWork.Complete();

                    BandLocationLeaderPeriodEmployeePeriod subPeriod = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetSubPeriodIncludeAttendace(bandLocationLeaderPeriodEmployeePeriodId, HttpContext.GetBusinessId());
                    if (subPeriod != null)
                    {
                        SubPeriodDetailDto subPeriodDetailDto = _mapper.Map<SubPeriodDetailDto>(subPeriod);
                        subPeriodDetailDto.Remainder = subPeriodDetailDto.TotalSalary - subPeriodDetailDto.TotalPaied - subPeriodDetailDto.TotalBorrow;
                        return Ok(subPeriodDetailDto);
                    }
                    return BadRequest();
                }
                return BadRequest();
            }
            return BadRequest();
        }

        // api/leaderEmployees/deleteSubPeriod/{bandLocationLeaderPeriodEmployeePeriodId}
        [HttpDelete("deleteSubPeriod/{bandLocationLeaderPeriodEmployeePeriodId}")]
        public async Task<IActionResult> DeleteSubPeriod([FromRoute] int? bandLocationLeaderPeriodEmployeePeriodId)
        {
            if(bandLocationLeaderPeriodEmployeePeriodId != null)
            {
                BandLocationLeaderPeriodEmployeePeriod entity = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.FindAsync(e => e.Id == bandLocationLeaderPeriodEmployeePeriodId);
                if(entity != null)
                {
                    entity.IsDeleted = true;
                    if(await _unitOfWork.Complete())
                    {
                        return NoContent();
                    }
                    return BadRequest();
                }
                return BadRequest();
            }
            return BadRequest();
        }

        // api/leaderEmployees/getEmployeesForAssign/{bandLocationLeaderPeriodId}
        [HttpGet("getEmployeesForAssign/{bandLocationLeaderPeriodId}")]
        public async Task<IActionResult> GetEmployeesForAssign([FromRoute] int? bandLocationLeaderPeriodId) 
        {
            if (bandLocationLeaderPeriodId != null) 
            {
                var employees = await _unitOfWork.Employees.GetAllIncludeTypes(HttpContext.GetBusinessId());
                var periodEmployees = await _unitOfWork.BandLocationLeaderPeriodEmployees.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && !e.IsDeleted);
                List<Employee> filterEmployees = new();

                foreach(Employee emp in employees) 
                {
                    var exist = periodEmployees.FirstOrDefault(e => e.EmployeeId == emp.Id);
                    if(exist == null) 
                    {
                        filterEmployees.Add(emp);
                    }
                }

                return Ok(_mapper.Map<IEnumerable<EmployeePageDto>>(filterEmployees));
            }
            return NotFound();
        }

    }
}

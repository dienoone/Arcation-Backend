﻿using Arcation.API.Extentions;
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
    public class AttendancesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        // bandLocationLeaderPeriodId

        public AttendancesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // api/attendances/{bandLocationLeaderPeriodId} => get for attendances page in side BandLocationLeaderPeriod:
        [HttpGet("{bandLocationLeaderPeriodId}")]
        public async Task<IActionResult> GetAll([FromRoute] int? bandLocationLeaderPeriodId)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                IEnumerable<Attendance> attendances = await _unitOfWork.Attendances.FindAllAsync(e => e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId, null, e => e.ended);
                if (attendances != null)
                {
                    return Ok(_mapper.Map<IEnumerable<AllAttendances>>(attendances));
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/attendances/{bandLocationLeaderPeriodId}/Attendance/{attendanceId} => get for select specific attendance:
        [HttpGet("{bandLocationLeaderPeriodId}/Attendance/{attendanceId}")]
        public async Task<IActionResult> GetSingelAttendance([FromRoute] int? bandLocationLeaderPeriodId, [FromRoute] int? attendanceId)
        {
            if (bandLocationLeaderPeriodId != null && attendanceId != null)
            {
                Attendance attendance = await _unitOfWork.Attendances.GetAttendance(attendanceId, bandLocationLeaderPeriodId);
                if (attendance != null)
                {
                    List<AttendanceEmployeeDto> attendanceDto = AttendanceHelper(attendance);
                    return Ok(attendanceDto);
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/attendances/InitializeAttendance/{bandLocationLeaderPeriodId} => InitializeAttendace:
        [HttpPost("InitializeAttendance/{bandLocationLeaderPeriodId}")]
        public async Task<IActionResult> InitializeAttendance([FromRoute] int? bandLocationLeaderPeriodId)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                string busienssID = HttpContext.GetBusinessId();
                string userID = HttpContext.GetUserId();
                var bandLocationLeaderPeriod = await _unitOfWork.BandLocationLeaderPeriods.FindAsync(e => e.Id == bandLocationLeaderPeriodId && !e.IsDeleted && e.BusinessId == busienssID);
                var attendance = await _unitOfWork.Attendances.FindAsync(e => e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && !e.ended && e.BusinessId == busienssID && e.BandLocationLeaderPeriod.PeriodId == bandLocationLeaderPeriod.PeriodId);
                if (bandLocationLeaderPeriod != null && attendance == null)
                {
                    Attendance newAttendance = new()
                    {
                        BandLocationLeaderPeriodId = bandLocationLeaderPeriod.Id,
                        BorrowValue = 0,
                        AttendanceState = false,
                        ended = false,
                        Date = DateTime.UtcNow,
                        WorkingHours = 0,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userID,
                        BusinessId = busienssID
                    };
                    var result = await _unitOfWork.Attendances.AddAsync(newAttendance);

                    if (result != null && await _unitOfWork.Complete())
                    {
                        List<int> neededList = new List<int>();
                        var employees = await _unitOfWork.BandLocationLeaderPeriodEmployees.GetForInitializeAttendance(bandLocationLeaderPeriodId, busienssID);
                        foreach (var employee in employees)
                        {
                            if (employee.BandLocationLeaderPeriodEmployeePeriods.Any(p => p.State))
                            {
                                var period = employee.BandLocationLeaderPeriodEmployeePeriods.FirstOrDefault(e => e.State);
                                neededList.Add(period.Id);
                            }
                            else
                            {
                                var newPeriod = new BandLocationLeaderPeriodEmployeePeriod
                                {
                                    StartingDate = DateTime.UtcNow,
                                    EndingDate = null,
                                    State = true,
                                    PayiedState = false,
                                    TotalBorrow = 0,
                                    EmployeeSalary = employee.Employee.Salary,
                                    IsDeleted = false,
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedBy = userID,
                                    BusinessId = busienssID,
                                    BandLocationLeaderPeriodEmployeeId = employee.Id
                                };
                                var isAdded = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.AddAsync(newPeriod);
                                if (isAdded != null && await _unitOfWork.Complete())
                                {
                                    neededList.Add(isAdded.Id);
                                }
                            }
                        }

                        if (neededList.Count > 0)
                        {
                            List<BandLocationLeaderPeriodEmployeePeriodAttendance> newlist = new List<BandLocationLeaderPeriodEmployeePeriodAttendance>();
                            foreach (var id in neededList)
                            {
                                var ha = new BandLocationLeaderPeriodEmployeePeriodAttendance
                                {
                                    AttendanceId = result.Id,
                                    BandLocationLeaderPeriodEmployeePeriodId = id,
                                    State = false,
                                    BorrowValue = 0,
                                    WorkingHours = 0,
                                    IsDeleted = false,
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedBy = userID,
                                    BusinessId = busienssID
                                };
                                newlist.Add(ha);
                            }

                            if (newlist.Count > 0)
                            {
                                var lastResult = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.AddRangeAsync(newlist);
                                if (lastResult != null)
                                {
                                    if (await _unitOfWork.Complete())
                                    {
                                        var finalResult = await _unitOfWork.Attendances.GetInializeAttendance(newAttendance.Id);
                                        return Ok(_mapper.Map<AttendanceDto>(newAttendance));
                                    }
                                }
                                return BadRequest("KeroKero");
                            }
                            return NoContent();
                        }

                        Attendance dto = await _unitOfWork.Attendances.GetAttendance(result.Id, bandLocationLeaderPeriodId);
                        List<AttendanceEmployeeDto> attendanceDto = AttendanceHelper(dto);
                        return Ok(attendanceDto);
                    }
                    return BadRequest();
                }
                return BadRequest("There are an attendance");
            }
            return BadRequest("WhatNow");
        }

        // api/attendances/TakeAttendance/{attendanceId} => get Attendance:
        [HttpPost("TakeAttendance/{attendanceId}")]
        public async Task<IActionResult> TakeAttendance([FromRoute] int? attendanceId, [FromBody] List<AttendanceEmployeeDto> dto)
        {
            if (attendanceId != null)
            {
                if (ModelState.IsValid)
                {
                    Attendance queryAttendance = await _unitOfWork.Attendances.TakeAttendance(attendanceId);
                    if (queryAttendance != null)
                    {
                        foreach (AttendanceEmployeeDto emp in dto) 
                        {
                            if (emp.IsLeader)
                            {
                                var attendance = await _unitOfWork.Attendances.FindAsync(e => e.Id == queryAttendance.Id && e.BandLocationLeaderPeriodId == emp.BandLocationLeaderPeriodEmployeePeriodAttendaceId);
                                if (attendance != null)
                                {
                                    queryAttendance.AttendanceState = emp.AttendanceState;
                                    queryAttendance.BorrowValue = emp.BorrowValue;
                                    queryAttendance.WorkingHours = emp.WorkingHours;
                                }
                                else 
                                {
                                    return NotFound("BandLocationLeaderPeriodId not found");
                                }
                            }
                            else
                            {
                                var employeeAttendance = queryAttendance.BandLocationLeaderPeriodEmployeePeriodAttendances.FirstOrDefault(e => e.Id == emp.BandLocationLeaderPeriodEmployeePeriodAttendaceId);
                                if (employeeAttendance != null)
                                {
                                    employeeAttendance.State = emp.AttendanceState;
                                    employeeAttendance.WorkingHours = emp.WorkingHours;
                                    employeeAttendance.BorrowValue = emp.BorrowValue;
                                }
                                else 
                                {
                                    return NotFound("BandLocationLeaderPeriodEmployeePeriodAttendanceId not found");
                                }
                            }
                        }

                        queryAttendance.ended = true;
                        await _unitOfWork.Complete();

                        var finalResult = await _unitOfWork.Attendances.GetAttendance(attendanceId, queryAttendance.BandLocationLeaderPeriodId);
                        List<AttendanceEmployeeDto> attendanceDto = AttendanceHelper(finalResult);
                        return Ok(attendanceDto);

                    }
                    return NotFound();
                }
                return BadRequest(ModelState);
            }
            return NotFound();
        }

        // api/attendances/{attendanceId} => Update Date Only:
        [HttpPut("{attendanceId}")]
        public async Task<IActionResult> UpdateAttendance([FromRoute] int? attendanceId, [FromBody] UpdateAttendance attendance) 
        {
            if (ModelState.IsValid)
            {
                if (attendanceId != null)
                {
                    Attendance queryAttendance = await _unitOfWork.Attendances.FindAsync(e => e.Id == attendanceId && e.BusinessId == HttpContext.GetBusinessId());
                    if (queryAttendance != null)
                    {
                        queryAttendance.Date = attendance.Date;
                        if(await _unitOfWork.Complete())
                        {
                            var finalResult = await _unitOfWork.Attendances.GetAttendance(queryAttendance.Id, queryAttendance.BandLocationLeaderPeriodId);
                            List<AttendanceEmployeeDto> attendanceDto = AttendanceHelper(finalResult);
                            return Ok(attendanceDto);
                        }
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/attendances/{attendanceId} => Delete Attendance:
        [HttpDelete("{attendanceId}")]
        public async Task<IActionResult> DeleteAttendance([FromRoute] int? attendanceId)
        {
            if(attendanceId != null)
            {
                Attendance queryAttendance = await _unitOfWork.Attendances.FindAsync(e => e.Id == attendanceId);
                if(queryAttendance != null)
                {
                    queryAttendance.IsDeleted = true;
                    if(await _unitOfWork.Complete())
                    {
                        return NoContent();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }


        private List<AttendanceEmployeeDto> AttendanceHelper(Attendance attendance)
        {
            List<AttendanceEmployeeDto> attendanceDto = new();

            AttendanceEmployeeDto leaderDto = new AttendanceEmployeeDto
            {
                BandLocationLeaderPeriodEmployeePeriodAttendaceId = attendance.BandLocationLeaderPeriodId,
                EmployeeName = attendance.BandLocationLeaderPeriod.BandLocationLeader.Leader.Name,
                EmployeeType = "Leader",
                AttendanceState = attendance.AttendanceState,
                WorkingHours = attendance.WorkingHours,
                BorrowValue = attendance.BorrowValue,
                IsLeader = true
            };

            attendanceDto.Add(leaderDto);

            foreach (var employee in attendance.BandLocationLeaderPeriodEmployeePeriodAttendances.Where(e => !e.IsDeleted))
            {
                AttendanceEmployeeDto dto = new AttendanceEmployeeDto
                {
                    BandLocationLeaderPeriodEmployeePeriodAttendaceId = employee.Id,
                    EmployeeName = employee.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.Employee.Name,
                    EmployeeType = employee.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.Employee.Type.Type,
                    AttendanceState = employee.State,
                    BorrowValue = employee.BorrowValue,
                    WorkingHours = employee.WorkingHours,
                    IsLeader = false
                };
                attendanceDto.Add(dto);
            }

            return attendanceDto;
        }
       

    }
}

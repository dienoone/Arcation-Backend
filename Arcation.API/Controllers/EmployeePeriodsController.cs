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
    public class EmployeePeriodsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        // bandLocationLeaderPeriodEmployeePeriodId
        // bandLocationLeaderPeriodId
        // Employee Period When Leader Need To Finish Payied Value:

        public EmployeePeriodsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        // api/BandLocationEmployees/{bandLocationLeaderPeriodId}
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

        // api/BandLocationEmployees/{bandLocationLeaderPeriodId}/EmployeePeriods/{Id}
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



        
        #region Helper:

        // Update BandLocationLeaderPeriodEmployee Tabel: Return New List And Delete Old List
        private async Task<List<BandLocationLeaderPeriodEmployee>> UpdateBandLocationLeaderPeriodEmployeeHelper(UpdateBandLocationLeaderPeriodEmployeeDto dto, BandLocationLeaderPeriodEmployee query, List<int> oldlist)
        {
            var newList = new List<int>();
            var deletedList = new List<int>();
            var BandLocationLeaderPeriodEmployees = new List<BandLocationLeaderPeriodEmployee>();

            for (int i = 0; i < dto.EmployeeIds.Count; i++)
            {
                if (!oldlist.Contains(dto.EmployeeIds[i]))
                {
                    newList.Add(dto.EmployeeIds[i]);
                }
            }

            for (int i = 0; i < oldlist.Count; i++)
            {
                if (!dto.EmployeeIds.Contains(oldlist[i]))
                {
                    deletedList.Add(oldlist[i]);
                }
            }

            if (newList != null)
            {

                foreach (int newid in newList)
                {
                    var bandLocationLeaderPeriodEmployee = new BandLocationLeaderPeriodEmployee
                    {
                        EmployeeId = newid,
                        BandLocationLeaderPeriodId = query.Id
                    };
                    BandLocationLeaderPeriodEmployees.Add(bandLocationLeaderPeriodEmployee);
                }
            }

            if (deletedList != null)
            {
                foreach (int deletedId in deletedList)
                {
                    var queryBandLocation = await _unitOfWork.BandLocationLeaderPeriodEmployees.FindAsync(e => e.Id == query.Id && e.EmployeeId == deletedId);
                    queryBandLocation.State = false;
                }
            }

            return BandLocationLeaderPeriodEmployees;
        }

        #endregion

        #region IsNeeded:

        // api/bandLocationEmployees
        [HttpPost]
        public async Task<IActionResult> CreateBandLocationLeaderPeriodEmployee([FromBody] AddBandLocationLeaderPeriodEmployeeDto dto)
        {
            if (ModelState.IsValid)
            {
                List<BandLocationLeaderPeriodEmployee> bandLocationLeaderPeriodEmployees = new List<BandLocationLeaderPeriodEmployee>();
                if (dto.EmployeeIds != null)
                {
                    foreach (int employeeId in dto.EmployeeIds)
                    {
                        BandLocationLeaderPeriodEmployee bandLocationLeaderPeriodEmployee = new BandLocationLeaderPeriodEmployee
                        {
                            EmployeeId = employeeId,
                            BandLocationLeaderPeriodId = dto.BandLocationLeaderPeriodId,
                            State = true,
                            IsDeleted = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = HttpContext.GetUserId(),
                            BusinessId = HttpContext.GetBusinessId()
                        };
                        bandLocationLeaderPeriodEmployees.Add(bandLocationLeaderPeriodEmployee);
                    }

                    if (bandLocationLeaderPeriodEmployees != null)
                    {
                        IEnumerable<BandLocationLeaderPeriodEmployee> isAdded = await _unitOfWork.BandLocationLeaderPeriodEmployees.AddRangeAsync(bandLocationLeaderPeriodEmployees);
                        if (isAdded != null && await _unitOfWork.Complete())
                        {
                            return Ok(isAdded);
                        }
                        return BadRequest("لم تتم عمليه الاضافه بنجاح");
                    }
                    return BadRequest();
                }
                return BadRequest();
            }
            return BadRequest(ModelState);
        }

        // api/bandLocationEmployees/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBandLocationLeaderPeriodEmployee([FromRoute] int? id, UpdateBandLocationLeaderPeriodEmployeeDto dto)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    BandLocationLeaderPeriodEmployee query = await _unitOfWork.BandLocationLeaderPeriodEmployees.FindAsync(c => c.Id == id && c.BusinessId == HttpContext.GetBusinessId());

                    if (query != null)
                    {

                        List<BandLocationLeaderPeriodEmployee> BandLocationLeaderPeriodEmployees = await UpdateBandLocationLeaderPeriodEmployeeHelper(dto, query, dto.oldList);
                        if (BandLocationLeaderPeriodEmployees != null)
                        {
                            await _unitOfWork.BandLocationLeaderPeriodEmployees.AddRangeAsync(BandLocationLeaderPeriodEmployees);
                        }

                        if (await _unitOfWork.Complete())
                        {
                            return Ok(query);
                        }
                        return BadRequest();
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/bandLocationEmployees/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> StopBandLocationLeaderPeriodEmployee([FromRoute] int? id)
        {
            if (id != null)
            {
                BandLocationLeaderPeriodEmployee query = await _unitOfWork.BandLocationLeaderPeriodEmployees.FindAsync(c => c.Id == id && c.BusinessId == HttpContext.GetBusinessId());

                if (query != null)
                {
                    query.State = false;
                    _unitOfWork.BandLocationLeaderPeriodEmployees.Update(query);
                    await _unitOfWork.Complete();
                    return NoContent();
                }

                return NotFound();
            }
            return NotFound();
        }

        #endregion

    }
}

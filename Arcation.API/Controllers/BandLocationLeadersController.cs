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
    public class BandLocationLeadersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BandLocationLeadersController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // api/bandlocationleaders/{bandLocationId}
        [HttpGet("{bandLocationId}")]
        public async Task<IActionResult> GetAll([FromRoute]int? bandLocationId)
        {
            try
            {
                if (bandLocationId != null)
                {
                    IEnumerable<BandLocationLeader> leaders = await _unitOfWork.BandLocationLeaders.GetLeadersWithPeriods(bandLocationId, HttpContext.GetBusinessId());
                    return Ok(_mapper.Map<IEnumerable<BandLocationLeaderPeriodsDto>>(leaders));
                }
                return BadRequest();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message + " ------ " +e.InnerException);
            }
        }

        // api/bandlocationleaders/{bandLocationId}/bandLocationLeader/{bandLocationLeaderId}
        [HttpGet("{bandLocationId}/bandLocationLeader/{bandLocationLeaderId}")]
        public async Task<IActionResult> GetAll([FromRoute] int? bandLocationId, [FromRoute] int? bandLocationLeaderId)
        {
            if (bandLocationId != null && bandLocationLeaderId != null)
            {
                BandLocationLeader leaders = await _unitOfWork.BandLocationLeaders.GetLeaderWithPeriod(bandLocationId,bandLocationLeaderId, HttpContext.GetBusinessId());
                return Ok(_mapper.Map<BandLocationLeaderPeriodsDto>(leaders));
            }
            return BadRequest();
        }


        /// <summary>
        /// assignLeaderToBand
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        // api/bandlocationleaders
        [HttpPost]
        public async Task<IActionResult> AssignLeaderToBand([FromBody] AddBandLeaderDto dto)
        {
            if (ModelState.IsValid)
            {
                // Check If Leader Is Exist OR not : 
                BandLocationLeader isExist = await _unitOfWork.BandLocationLeaders.FindAsync(bll => bll.BandLocationId == dto.BandLocationId && bll.LeaderId == dto.LeaderId);
                if(isExist == null)
                {
                    // Create New BandLocationLeader: 
                    BandLocationLeader newBandLocationLeader = new BandLocationLeader
                    {
                        LeaderId = dto.LeaderId,
                        BandLocationId = dto.BandLocationId,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = HttpContext.GetUserId(),
                        BusinessId = HttpContext.GetBusinessId()
                    };
                    // Add New BandLocationLeader : 
                    BandLocationLeader isAdded = await _unitOfWork.BandLocationLeaders.AddAsync(newBandLocationLeader);
                    // Check If BandLocationLeader Is IsAdded Or Not: 
                    if (isAdded != null && await _unitOfWork.Complete())
                    {
                        Leader leader = await _unitOfWork.Leaders.FindAsync(e => e.Id == dto.LeaderId);
                        // Assign Periods To BandLocationLeader : 
                        List<BandLocationLeaderPeriod> bandLocationLeaderPeriods = new List<BandLocationLeaderPeriod>();
                        if (dto.PeriodIds != null)
                        {
                            foreach (int periodId in dto.PeriodIds)
                            {
                                BandLocationLeaderPeriod issExist = await _unitOfWork.BandLocationLeaderPeriods.FindAsync(bllp => bllp.PeriodId == periodId && bllp.BandLocationLeaderId == isAdded.Id);
                                if (issExist == null)
                                {
                                    BandLocationLeaderPeriod bandLocationLeaderPeriod = new BandLocationLeaderPeriod
                                    {
                                        BandLocationLeaderId = isAdded.Id,
                                        PeriodId = periodId,
                                        StartingDate = DateTime.UtcNow,
                                        State = true,
                                        PayiedState = false,
                                        EndingDate = null,
                                        LeaderSalary = leader.Salary,
                                        TotalPaied = 0,
                                        IsDeleted = false,
                                        CreatedAt = DateTime.UtcNow,
                                        CreatedBy = HttpContext.GetUserId(),
                                        BusinessId = HttpContext.GetBusinessId()
                                    };
                                    bandLocationLeaderPeriods.Add(bandLocationLeaderPeriod);
                                }
                            }

                            if (bandLocationLeaderPeriods.Count > 0)
                            {
                                var result = await _unitOfWork.BandLocationLeaderPeriods.AddRangeAsync(bandLocationLeaderPeriods);

                                if (result != null && await _unitOfWork.Complete())
                                {
                                    BandLocationLeader queryLeader = await _unitOfWork.BandLocationLeaders.GetLeaderWithPeriod(dto.BandLocationId, isAdded.Id, HttpContext.GetBusinessId());
                                    return Ok(_mapper.Map<BandLocationLeaderPeriodsDto>(queryLeader));
                                }
                                return BadRequest("حدث خطأ اثناء الاضافه");
                            }
                            else
                            {
                                BandLocationLeader queryLeader = await _unitOfWork.BandLocationLeaders.GetLeaderWithPeriod(dto.BandLocationId, isAdded.Id, HttpContext.GetBusinessId());
                                return Ok(_mapper.Map<BandLocationLeaderPeriodsDto>(queryLeader));
                            }
                        }
                        return BadRequest();
                    }
                    return BadRequest();
                }
                return BadRequest();
            }
            return BadRequest(ModelState);
        }

        // api/bandlocationleaders/{Id}
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteBandLocationLeader([FromRoute] int? Id)
        {
            if(Id != null)
            {
                BandLocationLeader queryBandLocationLeader = await _unitOfWork.BandLocationLeaders.FindAsync(bll => bll.Id == Id);
                if(queryBandLocationLeader != null)
                {
                    queryBandLocationLeader.IsDeleted = true;
                    _unitOfWork.BandLocationLeaders.Update(queryBandLocationLeader);
                    await _unitOfWork.Complete();
                    return NoContent();
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/bandlocationleaders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeaderPeriods([FromRoute] int? id, [FromBody] UpdateBandLocationLeaderPeriodsDto dto)
        {
            if(id != null)
            {
                var leader = await _unitOfWork.BandLocationLeaders.GetLeaderWithPeriod(dto.BandLocationLeaderId, HttpContext.GetBusinessId());
                if(leader != null)
                {
                    var newList = await UpdateBandLocationLeaderPeriodHelper(dto, leader, dto.OldList);
                    if (newList != null)
                    {
                        var newBandLocationLeaderPeriods = await _unitOfWork.BandLocationLeaderPeriods.AddRangeAsync(newList);
                        if (newBandLocationLeaderPeriods != null && await _unitOfWork.Complete())
                        {
                            BandLocationLeader leaders = await _unitOfWork.BandLocationLeaders.GetLeaderWithPeriod(leader.BandLocationId, leader.Id, HttpContext.GetBusinessId());
                            return Ok(_mapper.Map<BandLocationLeaderPeriodsDto>(leaders));
                        }
                        return BadRequest();
                    }
                    else
                    {
                        if(await _unitOfWork.Complete())
                        {
                            BandLocationLeader leaders = await _unitOfWork.BandLocationLeaders.GetLeaderWithPeriod(leader.BandLocationId, leader.Id, HttpContext.GetBusinessId());
                            return Ok(_mapper.Map<BandLocationLeaderPeriodsDto>(leaders));
                        }
                        return BadRequest();
                    }
                    
                }
                return NotFound();
            }
            return NotFound();
        }

        #region Helper:

        // Update BandLocationLeaderPeriodTabel: Return New List And Delete Old List
        private async Task<List<BandLocationLeaderPeriod>> UpdateBandLocationLeaderPeriodHelper(UpdateBandLocationLeaderPeriodsDto dto, BandLocationLeader query, List<int> oldlist)
        {
            var newList = new List<int>(); // Ids For Create New List:
            var deletedList = new List<int>();
            var BandLocationLeaderPeriods = new List<BandLocationLeaderPeriod>(); // NewList From Added -> retrun:

            // Catch New Ids For Comming List:
            for (int i = 0; i < dto.PeriodIds.Count; i++)
            {
                if (!oldlist.Contains(dto.PeriodIds[i]))
                {
                    newList.Add(dto.PeriodIds[i]);
                }
            }

            // Catch DeletedIds From Comming OldList :
            for (int i = 0; i < oldlist.Count; i++)
            {
                if (!dto.PeriodIds.Contains(oldlist[i]))
                {
                    deletedList.Add(oldlist[i]);
                }
            }

            if (newList != null)
            {
                foreach (int newid in newList)
                {
                    BandLocationLeaderPeriod isExist = await _unitOfWork.BandLocationLeaderPeriods.FindAsync(bllp => bllp.PeriodId == newid && bllp.BandLocationLeaderId == dto.BandLocationLeaderId);
                    if(isExist == null)
                    {
                        var bandLocationLeaderPeriod = new BandLocationLeaderPeriod
                        {
                            BandLocationLeaderId = query.Id,
                            PeriodId = newid,
                            IsDeleted = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = HttpContext.GetUserId(),
                            BusinessId = HttpContext.GetBusinessId(),
                            StartingDate = DateTime.UtcNow,
                            State = true,
                            PayiedState = false,
                            EndingDate = null,
                            LeaderSalary = query.Leader.Salary,
                            TotalPaied = 0
                        };
                        BandLocationLeaderPeriods.Add(bandLocationLeaderPeriod);
                    }
                }
            }

            if (deletedList != null)
            {
                foreach (int deletedId in deletedList)
                {
                    var queryBandLocationLeaderPeriod = await _unitOfWork.BandLocationLeaderPeriods.FindAsync(e => e.Id == deletedId && e.BandLocationLeaderId == query.Id);
                    queryBandLocationLeaderPeriod.IsDeleted = false;
                }
            }

            return BandLocationLeaderPeriods;
        }

        #endregion

        #region GetForAdd

        // api/bandlocationleaders/{bandLocationId}/getLeadersForAdd
        [HttpGet("{bandLocationId}/getLeadersForAdd")]
        public async Task<IActionResult> GetLeadersForAdd([FromRoute] int? bandLocationId)
        {
            if(bandLocationId != null)
            {
                IEnumerable<Leader> entities = await _unitOfWork.Leaders.GetForAdd(HttpContext.GetBusinessId());
                List<Leader> filter = new();
                foreach(var leader in entities)
                {
                    var fil = leader.BandLocationLeaders.Any(e => e.BandLocationId == bandLocationId && !e.IsDeleted);
                    if(!fil)
                    {
                        filter.Add(leader);
                    }
                }

                IEnumerable<Period> periods = await _unitOfWork.Periods.FindAllAsync(e => e.BandLocationId == bandLocationId && !e.IsDeleted && e.State);

                AddLeaderPeriod addLeaderPeriod = new AddLeaderPeriod 
                {
                    Leaders = _mapper.Map<IEnumerable<AddLeaderToBandRequireDto>>(filter),
                    Periods = _mapper.Map<IEnumerable<AddPeriodRequireDto>>(periods)
                };                
                return Ok(addLeaderPeriod);
            }
            return NotFound();
        }

        // api/bandlocationleaders/{bandLocationId}/getPeriodsToAssign
        [HttpGet("{bandLocationId}/getPeriodsToAssign")]
        public async Task<IActionResult> GetPeriodsToAssingForLeader([FromRoute] int? bandLocationId)
        {
            if(bandLocationId != null)
            {
                IEnumerable<Period> periods = await _unitOfWork.Periods.FindAllAsync(e => e.BandLocationId == bandLocationId && !e.IsDeleted && !e.State);
                if(periods != null)
                {
                    return Ok(_mapper.Map<IEnumerable<AddPeriodRequireDto>>(periods));
                }
                return NotFound();
            }
            return NotFound();
        }


        // AssignPeriodToLeader:
        // api/bandlocationleaders/assignPeriodsToLeader
        [HttpPost("AssignPeriodsToLeader")]
        public async Task<IActionResult> AssignPeriodsToLeader([FromBody] AssignPeriodToLeaderDto dto)
        {
            if (ModelState.IsValid)
            {
                List<BandLocationLeaderPeriod> bandLocationLeaderPeriods = new List<BandLocationLeaderPeriod>();
                if (dto.PeriodIds != null)
                {
                    foreach (int periodId in dto.PeriodIds)
                    {
                        BandLocationLeaderPeriod isExist = await _unitOfWork.BandLocationLeaderPeriods.FindAsync(bllp => bllp.PeriodId == periodId && bllp.BandLocationLeaderId == dto.BandLocationLeaderId);
                        if (isExist == null)
                        {
                            BandLocationLeaderPeriod bandLocationLeaderPeriod = new BandLocationLeaderPeriod
                            {
                                BandLocationLeaderId = dto.BandLocationLeaderId,
                                PeriodId = periodId,
                                IsDeleted = false,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = HttpContext.GetUserId(),
                                BusinessId = HttpContext.GetBusinessId(),
                                StartingDate = DateTime.UtcNow,
                                State = true,
                                PayiedState = false,
                                EndingDate = null
                            };
                            bandLocationLeaderPeriods.Add(bandLocationLeaderPeriod);
                        }
                    }

                    var result = await _unitOfWork.BandLocationLeaderPeriods.AddRangeAsync(bandLocationLeaderPeriods);

                    if (bandLocationLeaderPeriods.Count > 0)
                    {
                        if (result != null && await _unitOfWork.Complete())
                        {
                            return Ok(result);
                        }
                        return BadRequest("حدث خطأ اثناء الاضافه");
                    }
                    return NoContent();
                }
                return BadRequest();
            }
            return BadRequest();
        }

        #endregion

        // api/bandlocationleaders/getPeriodDetail/{bandLocationLeaderPeriodId}
        [Authorize(Roles = "Admin, User")]
        [HttpGet("getPeriodDetail/{bandLocationLeaderPeriodId}")]
        public async Task<IActionResult> GetPeriodDetail([FromRoute] int? bandLocationLeaderPeriodId)
        {
            if(bandLocationLeaderPeriodId != null)
            {
                BandLocationLeaderPeriod entity = await _unitOfWork.BandLocationLeaderPeriods.GetLeaderPeriodDetail(bandLocationLeaderPeriodId, HttpContext.GetBusinessId());
                if(entity != null)
                {


                    PeriodDetail periodDetail = _mapper.Map<PeriodDetail>(entity);
                    periodDetail.Remainder = periodDetail.TotalTransictions - periodDetail.TotalWesteds - periodDetail.TotalBorrows;

                    LeaderDetail leaderDetail = _mapper.Map<LeaderDetail>(entity);
                    leaderDetail.TotalSalary = leaderDetail.TotalDays * leaderDetail.LeaderSalary;
                    leaderDetail.TotalRemainder = leaderDetail.TotalSalary - leaderDetail.TotalPaied - leaderDetail.TotalBorrow;

                    EmployeesDetail employeesDetail = _mapper.Map<EmployeesDetail>(entity);
                    employeesDetail.TotalRemainder = employeesDetail.TotalSalaryOfEmployess - employeesDetail.TotalPaied;

                    // ----- Employee Types:
                    var employeesTypes = await _unitOfWork.EmployeeTypes.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId());
                    List<EmployeeTypesDetail> employeeTypesDetails = new List<EmployeeTypesDetail>();
                    foreach (var employeesType in employeesTypes)
                    {
                        var employees = entity.BandLocationLeaderPeriodEmployees.Where(e => e.Employee.TypeId == employeesType.Id);
                        if (employees.Count() > 0)
                        {
                            EmployeeTypesDetail detail = new EmployeeTypesDetail
                            {
                                TypeName = employeesType.Type,
                                TotalEmployess = employees.Count()
                            };
                            employeeTypesDetails.Add(detail);
                        }
                    }
                    employeesDetail.employeeTypesDetails = employeeTypesDetails;

                    BandLocationLeaderPeriodDetailDto dto = new BandLocationLeaderPeriodDetailDto
                    {
                        PeriodDetail = periodDetail,
                        LeaderDetail = leaderDetail,
                        EmployeesDetail = employeesDetail
                    };

                    return Ok(dto);

                }
                return NotFound();
            }
            return BadRequest();
        }

    }
}

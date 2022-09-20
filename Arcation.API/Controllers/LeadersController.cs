using Arcation.API.Extentions;
using Arcation.API.Handler;
using Arcation.Core;
using Arcation.Core.Interfaces;
using Arcation.Core.Models;
using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels;
using Arcation.Core.ViewModels.ArcationViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class LeadersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageHandler _imageHandler;
        private readonly AppURL _appURL;
        // Leaders Related To Admin : 
        // Leaders page :

        public LeadersController(IUnitOfWork unitOfWork, IMapper mapper, IAccountRepository accountRepository, UserManager<ApplicationUser> userManager, IImageHandler imageHandler, IOptions<AppURL> appUrl)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
            _userManager = userManager;
            _imageHandler = imageHandler;
            _appURL = appUrl.Value;
        }

        #region Main:

        // api/leaders 
        [HttpGet]
        public async Task<IActionResult> GetAllLeaders()
        {

            IEnumerable<Leader> entities = await _unitOfWork.Leaders.FindAllAsync(l => !l.IsDeleted && l.BusinessId == HttpContext.GetBusinessId());
            return Ok(_mapper.Map<IEnumerable<LeadersPageDto>>(entities));
        }

        // api/leaders/{leaderId}
        [HttpGet("{leaderId}", Name = "GetLeader")]
        public async Task<IActionResult> GetSingleLeader([FromRoute] string leaderId)
        {
            if (!string.IsNullOrEmpty(leaderId) && !string.IsNullOrWhiteSpace(leaderId))
            {
                Leader leader = await _unitOfWork.Leaders.FindAsync(e => e.Id == leaderId);
                if (leader != null)
                {
                    return Ok(_mapper.Map<LeaderInfoDto>(leader));
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Leaders => Create Leader And Create user at the same time
        [HttpPost]
        public async Task<IActionResult> CreateLeader([FromBody] AddLeaderDto model)
        {
            if (ModelState.IsValid)
            {
                model.BusinessId = HttpContext.GetBusinessId();
                var result = await _accountRepository.RegisterLeaderAsync(model);

                if (result.IsAuthenticated)
                {
                    // Create New Leader:
                    Leader leader = new Leader
                    {
                        Id = result.Message,
                        Name = model.LeaderName,
                        Phone = model.PhoneNumber,
                        Salary = model.Salary,
                        UserName = model.UserName,
                        Passwrod = model.Password,
                        BusinessId = HttpContext.GetBusinessId(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = HttpContext.GetUserId(),
                        IsDeleted = false
                    };
                    await _unitOfWork.Leaders.AddAsync(leader);
                    if (await _unitOfWork.Complete())
                    {
                        return CreatedAtRoute("GetLeader", new { controller = "Leaders", leaderId = leader.Id }, _mapper.Map<LeaderInfoDto>(leader));
                    }
                    return BadRequest("لم تتم عمليه الاضافه بنجاح"); // Status Code: 200 OK
                }
                return BadRequest(result.Message); // Status Code: 400 BadRequest!!
            }
            return BadRequest(ModelState); // Status Code: 400 BadRequest!!
        }

        // api/leaders/{id} => Update Leader
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeader([FromRoute] string id, [FromBody] UpdateLeaderDto dto)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(id) && !string.IsNullOrWhiteSpace(id))
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        var leader = await _unitOfWork.Leaders.FindAsync(l => l.Id == id);
                        if (leader != null)
                        {
                            // Update Leaders Tabel:
                            leader.Name = dto.LeaderName;
                            leader.Passwrod = dto.Password;
                            leader.Phone = dto.PhoneNumber;
                            leader.Salary = dto.Salary;

                            // Update Users Tabels:
                            user.FirstName = dto.LeaderName;
                            user.PhoneNumber = dto.PhoneNumber;

                            // Reset Password:
                            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                            var result = await _userManager.ResetPasswordAsync(user, token, dto.Password);

                            // 
                            if (result.Succeeded)
                            {
                                await _userManager.UpdateAsync(user);
                                await _unitOfWork.Complete();
                                return Ok(_mapper.Map<LeaderInfoDto>(leader));
                            }
                            return BadRequest();
                        }
                        return NotFound();
                    }
                    return NotFound();

                }
                return NotFound();

            }
            return BadRequest(ModelState);

        }

        // api/leaders/{id} => delete leader
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeader([FromRoute] string id)
        {
            if (id != null)
            {

                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var leader = await _unitOfWork.Leaders.FindAsync(l => l.Id == id);
                    if (leader != null)
                    {
                        // Update Leaders Tabel:
                        leader.IsDeleted = true;

                        user.IsActive = false;
                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            await _userManager.UpdateAsync(user);
                            await _unitOfWork.Complete();
                            return NoContent();
                        }
                        return BadRequest();

                    }
                    return NotFound();
                }
                return NotFound();

            }
            return NotFound();
        }
      
        #endregion

        #region LeaderDetail:

        // api/Leaders/leaderDetail/{leaderId}
        [HttpGet("leaderDetail/{leaderId}")]
        public async Task<IActionResult> GetLeaderDetail([FromRoute] string leaderId)
        {
            if (!string.IsNullOrEmpty(leaderId) && !string.IsNullOrWhiteSpace(leaderId))
            {
                string businessId = HttpContext.GetBusinessId();
                Leader queryLeader = await _unitOfWork.Leaders.GetLeaderDetail(leaderId, businessId);
                if (queryLeader != null)
                {
                    LeaderDetails leaderDetail = new();

                    /* var locations = await _unitOfWork.Locations.LocaionsWithBandsRelatedToLeader(leaderId, HttpContext.GetBusinessId());
                     var list = new List<LeaderLocations>();
                     var locationIds = queryLeader.BandLocationLeaders.Select(e => e.BandLocation.LocationId);

                     foreach (int LocationId in locationIds)
                     {
                         Location queryLocation = await _unitOfWork.Locations.FindAsync(e => e.Id == LocationId);
                         var bandIds = queryLeader.BandLocationLeaders.Where(e => e.BandLocation.LocationId == LocationId).Select(e => e.BandLocation.BandId);
                         List<LeaderLocationBands> leaderLocationBands = new();

                         foreach (int bandId in bandIds)
                         {

                             Band queryBand = await _unitOfWork.Bands.FindAsync(e => e.Id == bandId);
                             BandLocationLeader queryBandLocationLeader = queryLeader.BandLocationLeaders.FirstOrDefault(e => e.BandLocation.LocationId == LocationId && e.BandLocation.BandId == bandId);
                             LeaderLocationBands leaderLocationBands1 = new LeaderLocationBands
                             {
                                 BandId = bandId,
                                 BandName = queryBand.BandName,
                                 BandLocationLeaderId = queryBandLocationLeader.Id
                             };
                             leaderLocationBands.Add(leaderLocationBands1);
                         }

                         LeaderLocations result = new LeaderLocations
                         {
                             LocationName = queryLocation.LocationName,
                             LocationId = LocationId,
                             BandLocations = leaderLocationBands

                         };
                         list.Add(result);
                     }*/
                    List<LeaderLocations> Dtos = new();

                    List<int> locationIds = _unitOfWork.BandLocationLeaders.GetLocationIdsForLeader(queryLeader.Id, businessId);
                    foreach (int locationId in locationIds)
                    {
                        Location location = await _unitOfWork.Locations.FindAsync(e => e.Id == locationId && !e.IsDeleted);
                        if (location != null)
                        {
                            BandLocationLeader bandLocationLeader = _unitOfWork.BandLocationLeaders.GetForLeader(queryLeader.Id, locationId);
                            List<int> bandIds = _unitOfWork.BandLocationLeaders.GetBandIdsForLeaderLocation(locationId, queryLeader.Id);
                            List<LeaderLocationBands> leaderLocationBands = new List<LeaderLocationBands>();
                            foreach (int bandId in bandIds)
                            {
                                Console.WriteLine(bandId);
                                Band band = await _unitOfWork.Bands.FindAsync(e => e.Id == bandId && !e.IsDeleted);
                                if (band != null)
                                {
                                    LeaderLocationBands LeaderLocationBand = new LeaderLocationBands
                                    {
                                        BandLocationLeaderId = bandLocationLeader.Id,
                                        BandId = band.Id,
                                        BandName = band.BandName
                                    };
                                    leaderLocationBands.Add(LeaderLocationBand);
                                }
                            }

                            if (leaderLocationBands.Count > 0)
                            {
                                LeaderLocations leaderLocation = new LeaderLocations
                                {
                                    LocationId = locationId,
                                    LocationName = location.LocationName,
                                    BandLocations = leaderLocationBands
                                };
                                Dtos.Add(leaderLocation);
                            }
                        }
                    }

                    LeaderInfoDto leaderInfoDto = _mapper.Map<LeaderInfoDto>(queryLeader);

                    EmployeeBusinessDetailDto employeeBusinessDetailDto = _mapper.Map<EmployeeBusinessDetailDto>(queryLeader);
                    employeeBusinessDetailDto.TotalRemainder = employeeBusinessDetailDto.TotalSalary - employeeBusinessDetailDto.TotalPayied;

                    leaderDetail.BusinessDetail = employeeBusinessDetailDto;
                    leaderDetail.LeaderInfo = leaderInfoDto;
                    leaderDetail.LeaderLocations = Dtos;

                    return Ok(leaderDetail);
                }
                return NotFound();
            }
            return NotFound();
        }

        #region LeaderPeriods:

        // api/Leaders/LeaderPeriods/{bandLocationLeaderId}
        [HttpGet("LeaderPeriods/{bandLocationLeaderId}")]
        public async Task<IActionResult> GetLeaderPeriods([FromRoute] int? bandLocationLeaderId)
        {
            if (bandLocationLeaderId != null)
            {
                var queryLeader = await _unitOfWork.BandLocationLeaders.GetForLeaderDetail(bandLocationLeaderId, HttpContext.GetBusinessId());
                if (queryLeader != null)
                {
                    GlobalLeaderDetail global = _mapper.Map<GlobalLeaderDetail>(queryLeader);
                    LeaderLocationDetail leaderLocationDetail = _mapper.Map<LeaderLocationDetail>(queryLeader);
                    global.LeaderLocationDetail = leaderLocationDetail;
                    return Ok(global);
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Leaders/LaederPeriod/{bandLocationLeaderPeriodId}
        [HttpGet("LaederPeriod/{bandLocationLeaderPeriodId}")]
        public async Task<IActionResult> GetPeriodDetail([FromRoute] int? bandLocationLeaderPeriodId)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                var queryPeriod = await _unitOfWork.BandLocationLeaderPeriods.GetLeaderPeriodFinish(bandLocationLeaderPeriodId, HttpContext.GetBusinessId());
                if (queryPeriod != null)
                {
                    LeaderPeriodDetail leaderPeriodDetail = _mapper.Map<LeaderPeriodDetail>(queryPeriod);
                    leaderPeriodDetail.Remainder = leaderPeriodDetail.TotalSalary - leaderPeriodDetail.TotalPaied - leaderPeriodDetail.TotalBorrow;
                    return Ok(leaderPeriodDetail);
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Leaders/LaederPeriod/{bandLocationLeaderPeriodId}
        [HttpPut("LaederPeriod/{bandLocationLeaderPeriodId}")]
        public async Task<IActionResult> EditLeaderPeriod([FromRoute] int? bandLocationLeaderPeriodId, [FromBody] UpdateLeaderPeriod dto)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                if (ModelState.IsValid)
                {
                    BandLocationLeaderPeriod queryPeriod = await _unitOfWork.BandLocationLeaderPeriods.FindAsync(e => e.Id == bandLocationLeaderPeriodId && e.BusinessId == HttpContext.GetBusinessId());
                    if (queryPeriod != null)
                    {
                        queryPeriod.LeaderSalary = dto.LeaderSalary;
                        await _unitOfWork.Complete();
                        var result = await _unitOfWork.BandLocationLeaderPeriods.GetLeaderPeriodFinish(bandLocationLeaderPeriodId, HttpContext.GetBusinessId());
                        if (result != null)
                        {
                            LeaderPeriodDetail leaderPeriodDetail = _mapper.Map<LeaderPeriodDetail>(result);
                            leaderPeriodDetail.Remainder = leaderPeriodDetail.TotalSalary - leaderPeriodDetail.TotalPaied - leaderPeriodDetail.TotalBorrow;
                            return Ok(leaderPeriodDetail);
                        }
                        return NotFound();
                    }
                    return NotFound();
                }
                return BadRequest(ModelState);
            }
            return NotFound();
        }

        // api/Leaders/finishLeaderPeriod/{bandLocationLeaderPeriodId}
        [HttpPost("finishLeaderPeriod/{bandLocationLeaderPeriodId}")]
        public async Task<IActionResult> FinishLeaderPeriod([FromRoute] int? bandLocationLeaderPeriodId, [FromBody] FinishLeaderPeriod dto)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                BandLocationLeaderPeriod bandLocationLeaderPeriod = await _unitOfWork.BandLocationLeaderPeriods.GetLeaderPeriodFinish(bandLocationLeaderPeriodId, HttpContext.GetBusinessId());
                if (bandLocationLeaderPeriod != null)
                {
                    double totalDays = 0;
                    double borrowValue = 0;
                    foreach (var attendance in bandLocationLeaderPeriod.Attendances)
                    {
                        totalDays += attendance.WorkingHours;
                        borrowValue += attendance.BorrowValue;
                    }

                    double employeeSalary = totalDays * bandLocationLeaderPeriod.LeaderSalary;

                    bandLocationLeaderPeriod.TotalPaied += dto.PaiedValue;
                    if (bandLocationLeaderPeriod.TotalPaied >= employeeSalary)
                    {
                        bandLocationLeaderPeriod.State = true;
                    }

                    if (await _unitOfWork.Complete())
                    {
                        var queryPeriod = await _unitOfWork.BandLocationLeaderPeriods.GetLeaderPeriodFinish(bandLocationLeaderPeriodId, HttpContext.GetBusinessId());
                        if (queryPeriod != null)
                        {
                            LeaderPeriodDetail leaderPeriodDetail = _mapper.Map<LeaderPeriodDetail>(queryPeriod);
                            leaderPeriodDetail.Remainder = leaderPeriodDetail.TotalSalary - leaderPeriodDetail.TotalPaied - leaderPeriodDetail.TotalBorrow;
                            return Ok(leaderPeriodDetail);
                        }
                        return NotFound();
                    }

                }
                return NotFound();
            }
            return NotFound();
        }

        #endregion

        #endregion

    }
}


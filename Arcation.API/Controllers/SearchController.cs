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
    public class SearchController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private string BusinessId;

        // TODO: ADD AUTHORIZE ATTRIBUTE:
        public SearchController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // api/Search/Company/{name}
        [HttpGet("Company/{name}")]
        public async Task<IActionResult> SearchCompany([FromRoute] string name)
        {
            BusinessId = HttpContext.GetBusinessId();
            if (name != null)
            {
                IEnumerable<Company> entities = await _unitOfWork.Companies.FindAllAsync(e => e.BusinessId == BusinessId && e.Name.Contains(name));
                return Ok(_mapper.Map<IEnumerable<CompanyViewModel>>(entities));
            }
            else
            {
                IEnumerable<Company> entities = await _unitOfWork.Companies.FindAllAsync(e => e.BusinessId == BusinessId);
                return Ok(_mapper.Map<IEnumerable<CompanyViewModel>>(entities));
            }
        }

        // api/Search/Company/{companyId}/Location/{name}
        [HttpGet("Company/{companyId}/Location/{name}")]
        public async Task<IActionResult> SearchLocation([FromRoute] int? companyId, [FromRoute] string name)
        {
            if (companyId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                if (name != null)
                {
                    IEnumerable<Location> entities = await _unitOfWork.Locations.SearchLocationAsync(BusinessId, companyId, name);
                    return Ok(_mapper.Map<IEnumerable<LocationViewModel>>(entities));
                }
                else
                {
                    IEnumerable<Location> entities = await _unitOfWork.Locations.GetAllLocationsAsync(BusinessId, companyId);
                    return Ok(_mapper.Map<IEnumerable<LocationViewModel>>(entities));
                }
            }
            return NotFound();
        }

        #region BandLocation :

        // api/Search/Bills/{bandLocationId}/Bill/{name}
        [HttpGet("Bills/{bandLocationId}/Bill/{name}")]
        public async Task<IActionResult> SearchBill([FromRoute] int? bandLocationId, [FromRoute] string name)
        {
            if (bandLocationId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                BandLocation bandLocation = await _unitOfWork.BandLocations.FindAsync(e => e.Id == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                if (bandLocation != null)
                {
                    if (name != null)
                    {
                        IEnumerable<Bill> entities = await _unitOfWork.Bills.FindAllAsync(e => e.BandLocationId == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted && e.BillCode.Contains(name));
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<BillDto>>(entities));
                        }
                        return NoContent();
                    }
                    else
                    {
                        IEnumerable<Bill> entities = await _unitOfWork.Bills.FindAllAsync(e => e.BandLocationId == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<BillDto>>(entities));
                        }
                        return NoContent();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Search/Periods/{bandLocationId}/period/{name}
        [HttpGet("Periods/{bandLocationId}/period/{name}")]
        public async Task<IActionResult> SearchPeriod([FromRoute] int? bandLocationId, [FromRoute] string name)
        {
            if (bandLocationId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                BandLocation bandLocation = await _unitOfWork.BandLocations.FindAsync(e => e.Id == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                if (bandLocation != null)
                {
                    if (name != null)
                    {
                        IEnumerable<Period> entities = await _unitOfWork.Periods.FindAllAsync(e => e.BandLocationId == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted && e.Name.Contains(name));
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<AllPeriodDto>>(entities));
                        }
                        return NoContent();
                    }
                    else
                    {
                        IEnumerable<Period> entities = await _unitOfWork.Periods.FindAllAsync(e => e.BandLocationId == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<AllPeriodDto>>(entities));
                        }
                        return NoContent();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Search/Incomes/{bandLocationId}/Income/{name}
        [HttpGet("Incomes/{bandLocationId}/Income/{name}")]
        public async Task<IActionResult> SearchIncome([FromRoute] int? bandLocationId, [FromRoute] string name)
        {
            if (bandLocationId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                BandLocation bandLocation = await _unitOfWork.BandLocations.FindAsync(e => e.Id == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                if (bandLocation != null)
                {
                    if (name != null)
                    {
                        IEnumerable<Income> entities = await _unitOfWork.Incomes.FindAllAsync(e => e.BandLocationId == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted && e.Estatement.Contains(name));
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<IncomeDto>>(entities));
                        }
                        return NoContent();
                    }
                    else
                    {
                        IEnumerable<Income> entities = await _unitOfWork.Incomes.FindAllAsync(e => e.BandLocationId == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<IncomeDto>>(entities));
                        }
                        return NoContent();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Search/BLWesteds/{bandLocationId}/Wested/{name}
        [HttpGet("BLWesteds/{bandLocationId}/Wested/{name}")]
        public async Task<IActionResult> SearchBLWestd([FromRoute] int? bandLocationId, [FromRoute] string name)
        {
            if (bandLocationId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                BandLocation bandLocation = await _unitOfWork.BandLocations.FindAsync(e => e.Id == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                if (bandLocation != null)
                {
                    if (name != null)
                    {
                        IEnumerable<BLWested> entities = await _unitOfWork.BLWesteds.FindAllAsync(e => e.BandLocationId == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted && e.Estatement.Contains(name));
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<BLWestedDto>>(entities));
                        }
                        return NoContent();
                    }
                    else
                    {
                        IEnumerable<BLWested> entities = await _unitOfWork.BLWesteds.FindAllAsync(e => e.BandLocationId == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<BLWestedDto>>(entities));
                        }
                        return NoContent();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Search/Extracts/{bandLocationId}/Extract/{name}
        [HttpGet("Extracts/{bandLocationId}/Extract/{name}")]
        public async Task<IActionResult> SearchExtract([FromRoute] int? bandLocationId, [FromRoute] string name)
        {
            if (bandLocationId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                BandLocation bandLocation = await _unitOfWork.BandLocations.FindAsync(e => e.Id == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                if (bandLocation != null)
                {
                    if (name != null)
                    {
                        IEnumerable<Extract> entities = await _unitOfWork.Extracts.SearchExtractAsync(bandLocationId, name, BusinessId);
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<AllExtracts>>(entities));
                        }
                        return NoContent();
                    }
                    else
                    {
                        IEnumerable<Extract> entities = await _unitOfWork.Extracts.FindAllAsync(e => e.BandLocationId == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<AllExtracts>>(entities));
                        }
                        return NoContent();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        #endregion

        #region BandLocationLeader:

        // api/Search/BLLeaders/{bandLocationId}/leader/{name}
        [HttpGet("BLLeaders/{bandLocationId}/leader/{name}")]
        public async Task<IActionResult> SearchBandLocationLeader([FromRoute] int? bandLocationId, [FromRoute] string name)
        {
            if (bandLocationId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                BandLocation bandLocation = await _unitOfWork.BandLocations.FindAsync(e => e.Id == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                if (bandLocation != null)
                {
                    if (name != null)
                    {
                        IEnumerable<BandLocationLeader> entities = await _unitOfWork.BandLocationLeaders.SearchLeadersWithPeriods(bandLocationId, name, BusinessId);
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<BandLocationLeaderPeriodsDto>>(entities));
                        }
                        return NoContent();
                    }
                    else
                    {
                        IEnumerable<BandLocationLeader> entities = await _unitOfWork.BandLocationLeaders.GetLeadersWithPeriods(bandLocationId, BusinessId);
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<BandLocationLeaderPeriodsDto>>(entities));
                        }
                        return NoContent();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Search/LeadersAssign/{bandLocationId}/leader/{name}
        [HttpGet("LeadersAssign/{bandLocationId}/leader/{name}")]
        public async Task<IActionResult> SearchLeaderForAdd([FromRoute] int? bandLocationId, [FromRoute] string name)
        {
            if (bandLocationId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                BandLocation bandLocation = await _unitOfWork.BandLocations.FindAsync(e => e.Id == bandLocationId && e.BusinessId == BusinessId && !e.IsDeleted);
                if (bandLocation != null)
                {
                    if (name != null)
                    {
                        IEnumerable<Leader> entities = await _unitOfWork.Leaders.SearchForAdd(name ,HttpContext.GetBusinessId());
                        List<Leader> filter = new();
                        foreach (var leader in entities)
                        {
                            var fil = leader.BandLocationLeaders.Any(e => e.BandLocationId == bandLocationId && !e.IsDeleted);
                            if (!fil)
                            {
                                filter.Add(leader);
                            }
                        }
                        return Ok(_mapper.Map<IEnumerable<AddLeaderToBandRequireDto>>(filter));
                    }
                    else
                    {
                        IEnumerable<Leader> entities = await _unitOfWork.Leaders.GetForAdd(HttpContext.GetBusinessId());
                        List<Leader> filter = new();
                        foreach (var leader in entities)
                        {
                            var fil = leader.BandLocationLeaders.Any(e => e.BandLocationId == bandLocationId && !e.IsDeleted);
                            if (!fil)
                            {
                                filter.Add(leader);
                            }
                        }
                        return Ok(_mapper.Map<IEnumerable<AddLeaderToBandRequireDto>>(filter));
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Search/LeaderTransictions/{bandLocationId}/transiction/{name}
        [HttpGet("LeaderTransictions/{bandLocationLeaderPeriodId}/transiction/{name}")]
        public async Task<IActionResult> SearchTransictions([FromRoute] int? bandLocationLeaderPeriodId, [FromRoute] string name)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                BandLocationLeaderPeriod bandLocationLeaderPeriod = await _unitOfWork.BandLocationLeaderPeriods.FindAsync(e => e.Id == bandLocationLeaderPeriodId && e.BusinessId == BusinessId && !e.IsDeleted);
                if (bandLocationLeaderPeriod != null)
                {
                    if (name != null)
                    {
                        IEnumerable<Transiction> entities = await _unitOfWork.LeaderTransactions.FindAllAsync(e => e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && e.BusinessId == BusinessId && !e.IsDeleted && e.Estatement.Contains(name));
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<TransictionDto>>(entities));
                        }
                        return NoContent();
                    }
                    else
                    {
                        IEnumerable<Transiction> entities = await _unitOfWork.LeaderTransactions.FindAllAsync(e => e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && e.BusinessId == BusinessId && !e.IsDeleted);
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<TransictionDto>>(entities));
                        }
                        return NoContent();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Search/LeaderWesteds/{bandLocationId}/wested/{name}
        [HttpGet("LeaderWesteds/{bandLocationLeaderPeriodId}/wested/{name}")]
        public async Task<IActionResult> SearchLeaderWesteds([FromRoute] int? bandLocationLeaderPeriodId, [FromRoute] string name)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                BandLocationLeaderPeriod bandLocation = await _unitOfWork.BandLocationLeaderPeriods.FindAsync(e => e.Id == bandLocationLeaderPeriodId && e.BusinessId == BusinessId && !e.IsDeleted);
                if (bandLocation != null)
                {
                    if (name != null)
                    {
                        IEnumerable<Wested> entities = await _unitOfWork.LeaderWesteds.FindAllAsync(e => e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && e.BusinessId == BusinessId && !e.IsDeleted && e.Estatement.Contains(name));
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<LeaderWestedDto>>(entities));
                        }
                        return NoContent();
                    }
                    else
                    {
                        IEnumerable<Wested> entities = await _unitOfWork.LeaderWesteds.FindAllAsync(e => e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && e.BusinessId == BusinessId && !e.IsDeleted);
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<LeaderWestedDto>>(entities));
                        }
                        return NoContent();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Search/LeaderEmployees/{bandLocationId}/employee/{name}
        [HttpGet("LeaderEmployees/{bandLocationLeaderPeriodId}/employee/{name}")]
        public async Task<IActionResult> SearchLeaderEmployees([FromRoute] int? bandLocationLeaderPeriodId, [FromRoute] string name)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                BandLocationLeaderPeriod bandLocation = await _unitOfWork.BandLocationLeaderPeriods.FindAsync(e => e.Id == bandLocationLeaderPeriodId && e.BusinessId == BusinessId && !e.IsDeleted);
                if (bandLocation != null)
                {
                    if (name != null)
                    {
                        IEnumerable<BandLocationLeaderPeriodEmployee> entities = await _unitOfWork.BandLocationLeaderPeriodEmployees.SearchPeriodEmployeeDataAsync(bandLocationLeaderPeriodId, name, BusinessId);
                        if (entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<BandLocationLeaderPeriodEmployeeDto>>(entities));
                        }
                        return NoContent();
                    }
                    else
                    {
                        IEnumerable<BandLocationLeaderPeriodEmployee> entities = await _unitOfWork.BandLocationLeaderPeriodEmployees.GetPeriodEmployeeDataAsync(bandLocationLeaderPeriodId, HttpContext.GetBusinessId());
                        if(entities != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<BandLocationLeaderPeriodEmployeeDto>>(entities));
                        }
                        return NoContent();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Search/LeaderEmployees/{bandLocationId}/employee/{name}
        [HttpGet("LeaderEmployeesForAdd/{bandLocationLeaderPeriodId}/employee/{name}")]
        public async Task<IActionResult> SearchLeaderEmployeesForAdd([FromRoute] int? bandLocationLeaderPeriodId, [FromRoute] string name)
        {
            if (bandLocationLeaderPeriodId != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                BandLocationLeaderPeriod bandLocation = await _unitOfWork.BandLocationLeaderPeriods.FindAsync(e => e.Id == bandLocationLeaderPeriodId && e.BusinessId == BusinessId && !e.IsDeleted);
                if (bandLocation != null)
                {
                    if (name != null)
                    {
                        IEnumerable<Employee> employees = await _unitOfWork.Employees.FindAllAsync(e => e.BusinessId == BusinessId && !e.IsDeleted && e.Name.Contains(name), new[] { "Type" });
                        IEnumerable<BandLocationLeaderPeriodEmployee> periodEmployees = await _unitOfWork.BandLocationLeaderPeriodEmployees.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && !e.IsDeleted);
                        List<Employee> filterEmployees = new();
                        foreach (Employee emp in employees)
                        {
                            var exist = periodEmployees.FirstOrDefault(e => e.EmployeeId == emp.Id);
                            if (exist == null)
                            {
                                filterEmployees.Add(emp);
                            }
                        }
                        if (filterEmployees != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<EmployeePageDto>>(filterEmployees));
                        }
                        return NoContent();
                    }
                    else
                    {
                        IEnumerable<Employee> employees = await _unitOfWork.Employees.FindAllAsync(e => e.BusinessId == BusinessId && !e.IsDeleted, new[] {"Type" });
                        IEnumerable<BandLocationLeaderPeriodEmployee> periodEmployees = await _unitOfWork.BandLocationLeaderPeriodEmployees.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && !e.IsDeleted);
                        List<Employee> filterEmployees = new();
                        foreach (Employee emp in employees)
                        {
                            var exist = periodEmployees.FirstOrDefault(e => e.EmployeeId == emp.Id);
                            if (exist == null)
                            {
                                filterEmployees.Add(emp);
                            }
                        }
                        if(filterEmployees != null)
                        {
                            return Ok(_mapper.Map<IEnumerable<EmployeePageDto>>(filterEmployees));
                        }
                        return NoContent();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Search/Attendances/{attendanceID}/Employee/{name}
        [HttpGet("Attendances/{attendanceID}/Employee/{name}")]
        public async Task<IActionResult> SearchAttendance([FromRoute] int? attendanceID, [FromRoute] string name) 
        {
            if (attendanceID != null)
            {
                BusinessId = HttpContext.GetBusinessId();
                Attendance attendanceExist = await _unitOfWork.Attendances.FindAsync(e => e.Id == attendanceID);
                if (attendanceExist != null) 
                {
                    if (name != null)
                    {
                        Attendance entities = await _unitOfWork.Attendances.GetSearchAttendance(attendanceID, name, BusinessId);
                        return Ok(_mapper.Map<AttendanceDto>(entities));
                    }
                    else
                    {
                        Attendance attendance = await _unitOfWork.Attendances.GetAttendance(attendanceID, attendanceExist.BandLocationLeaderPeriodId);
                        return Ok(_mapper.Map<AttendanceDto>(attendance));
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        #endregion


    }
}

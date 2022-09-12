using Arcation.API.Extentions;
using Arcation.Core;
using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using AutoMapper;
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
    public class GlobalReportsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private string businessID;

        public GlobalReportsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // api/GlobalReports => Reports Page:
        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            string businessId = HttpContext.GetBusinessId();
            IEnumerable<Company> companies = await _unitOfWork.Companies.FindAllAsync(e => e.BusinessId == businessId && !e.IsDeleted);
            if(companies != null)
            {
                var Ids = companies.Select(e => e.Id);
                List<GlobalAccountMoney> preGlobalAccountMoney = new();
                foreach (int id in Ids)
                {
                    preGlobalAccountMoney.Add(AccountMoneyCompany(id, businessId));
                }

                GlobalReport globalReport = new();
                GlobalAccount globalAccount = new();

                GlobalAccountWork globalAccountWork = await AccountWorkHelper(businessId);
                GlobalAccountMoney globalAccountMoney = AccountMoneyHelper(preGlobalAccountMoney);
                globalAccount.GlobalAccountMoney = globalAccountMoney;
                globalAccount.GlobalAccountWork = globalAccountWork;

                List<CompanyReprots> companyReprots = _mapper.Map<List<CompanyReprots>>(companies);

                globalReport.Companies = companyReprots;
                globalReport.globalAccount = globalAccount;

                return Ok(globalReport);
            }
            return NoContent();
        }

        // api/GlobalReports/{companyId}
        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetSingleCompanyReport([FromRoute] int? companyId)
        {
            if (companyId != null)
            {
                businessID = HttpContext.GetBusinessId();
                Company queryCompany = await _unitOfWork.Companies.FindAsync(e => e.Id == companyId && e.BusinessId == businessID && !e.IsDeleted);
                if(queryCompany != null)
                {
                    GlobalReport globalReport = new();
                    GlobalAccount globalAccount = new();
                    
                    globalAccount.GlobalAccountWork = await AccountWorkCompanyHelper(queryCompany, businessID);
                    globalAccount.GlobalAccountMoney = AccountMoneyCompany(companyId, businessID);

                    IEnumerable<Location> locations = await _unitOfWork.Locations.FindAllAsync(e => e.BusinessId == businessID && e.CompanyId == queryCompany.Id && !e.IsDeleted);
                    globalReport.Locations = _mapper.Map<IEnumerable<CompanyLocationsReports>>(locations);
                    globalReport.globalAccount = globalAccount;

                    return Ok(globalReport);
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/GlobalReports/Company/{locationId}
        [HttpGet("Company/{locationId}")]
        public async Task<IActionResult> GetSingleLocationReport([FromRoute] int? locationId)
        {
            if (locationId != null)
            {
                businessID = HttpContext.GetBusinessId();
                var location = await _unitOfWork.Locations.FindAsync(e => e.Id == locationId && e.BusinessId == businessID && !e.IsDeleted);
                if (location != null)
                {
                    GlobalReport globalReport = new();
                    GlobalAccount globalAccount = new();
                    var preBands = await _unitOfWork.BandLocations.FindAllAsync(e => e.LocationId == locationId && e.BusinessId == businessID && !e.IsDeleted, new[] { "Band" });

                    globalAccount.GlobalAccountMoney = AccountMoneyLocation(locationId, businessID);
                    globalAccount.GlobalAccountWork = await AccountWorkLocationHelper(location, businessID);

                    IEnumerable<CompanyLocationBandReports> bands = _mapper.Map<IEnumerable<CompanyLocationBandReports>>(preBands);

                    globalReport.Bands = bands;
                    globalReport.globalAccount = globalAccount;

                    return Ok(globalReport);
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/GlobalReports/Location/{bandId}
        [HttpGet("Location/{bandLocationId}")]
        public async Task<IActionResult> GetSingleBandReport([FromRoute] int? bandLocationId)
        {
            if (bandLocationId != null)
            {
                businessID = HttpContext.GetBusinessId();
                var band = await _unitOfWork.BandLocations.FindAsync(e => e.Id == bandLocationId && e.BusinessId == businessID && !e.IsDeleted);
                if (band != null)
                {
                    GlobalReport globalReport = new();
                    GlobalAccount globalAccount = new();

                    globalAccount.GlobalAccountMoney = AccountMoneyBand(bandLocationId, businessID);
                    globalAccount.GlobalAccountWork = await AccountWorkLocationBandHelper(band, businessID);

                    globalReport.globalAccount = globalAccount;

                    return Ok(globalReport);
                }
                return NotFound();
            }
            return NotFound();
        }

        #region Helpers:

        private async Task<GlobalAccountWork> AccountWorkHelper(string businessId)
        {
            GlobalAccountWork globalAccountWork = new GlobalAccountWork();

            // Types:
            var types = await _unitOfWork.EmployeeTypes.FindAllAsync(e => e.BusinessId == businessId);

            globalAccountWork.NumberOfLocations = await _unitOfWork.Locations.CountAsync(e => e.BusinessId == businessId);
            globalAccountWork.NumberOfCompanies = await _unitOfWork.Companies.CountAsync(e => e.BusinessId == businessId);
            globalAccountWork.NumberOfEmployees = await _unitOfWork.Employees.CountAsync(e => e.BusinessId == businessId);
            globalAccountWork.NumberOfBands = await _unitOfWork.Bands.CountAsync(e => e.BusinessId == businessId);
            globalAccountWork.NumberOfLeaders = await _unitOfWork.Leaders.CountAsync(e => e.BusinessId == businessId);
            globalAccountWork.NumberOfBands = await _unitOfWork.Bands.CountAsync(e => e.BusinessId == businessId);
            globalAccountWork.EmployeeTypesDetails = await GetTypesHelper(types);

            return globalAccountWork;
        }
        private async Task<GlobalAccountWork> AccountWorkCompanyHelper(Company company, string businessId)
        {
            GlobalAccountWork globalAccountWork = new();
            List<EmployeeTypesDetail> employeeTypesDetails = new();

            globalAccountWork.CompanyName = company.Name;
            globalAccountWork.NumberOfBands = _unitOfWork.BandLocations.NumberOfBandCompany(company.Id, businessId);
            globalAccountWork.NumberOfEmployees = _unitOfWork.BandLocationLeaderPeriodEmployees.GetNumberOfEmployeesInCompany(company.Id, businessId);
            globalAccountWork.NumberOfLeaders = _unitOfWork.BandLocationLeaders.NumberOfLeadersCompany(company.Id, businessId);
            globalAccountWork.NumberOfLocations = _unitOfWork.Locations.NumberOfLocationCompany(company.Id, businessId);

            var types = await _unitOfWork.BandLocationLeaderPeriodEmployees.GetEmployeeTypesCompany(company.Id);
            var filterdList = new List<EmployeeType>();

            foreach (var type in types)
            {
                if (!filterdList.Contains(type))
                {
                    filterdList.Add(type);
                }
            }

            foreach (var type in filterdList)
            {
                EmployeeTypesDetail employeeTypesDetail = new();
                employeeTypesDetail.TypeName = type.Type;
                employeeTypesDetail.TotalEmployess = _unitOfWork.BandLocationLeaderPeriodEmployees.GetNumberOfEmployeeTypeInCompany(company.Id , type.Id, businessId);
                employeeTypesDetails.Add(employeeTypesDetail);
            }

            globalAccountWork.EmployeeTypesDetails = employeeTypesDetails;

            return globalAccountWork;
        }

        private async Task<GlobalAccountWork> AccountWorkLocationHelper(Location location, string businessID)
        {
            List<EmployeeTypesDetail> employeeTypesDetails = new();
            GlobalAccountWork globalAccountWork = new();

            globalAccountWork.LocationName = location.LocationName;
            globalAccountWork.NumberOfBands = await _unitOfWork.BandLocations.CountAsync(e => e.LocationId == location.Id && e.BusinessId == businessID && !e.IsDeleted);
            globalAccountWork.NumberOfEmployees = _unitOfWork.BandLocationLeaderPeriodEmployees.GetNumberOfEmployeesInLocation(location.Id, businessID);
            globalAccountWork.NumberOfLeaders = _unitOfWork.BandLocationLeaders.NumberOfLeadersLocation(location.Id, businessID);
            globalAccountWork.StartingDate = location.StartingDate;
            globalAccountWork.EndingDate = location.EndingDate;
            Location queryLocation = await _unitOfWork.Locations.FindAsync(e => e.Id == location.Id, new[] { "Company" });
            globalAccountWork.CompanyName = queryLocation.Company.Name;

            var types = await _unitOfWork.BandLocationLeaderPeriodEmployees.GetEmployeeTypesLocation(location.Id);
            var filterdList = new List<EmployeeType>();
            foreach (var type in types)
            {
                if (!filterdList.Contains(type))
                {
                    filterdList.Add(type);
                }
            }
            foreach (var type in filterdList)
            {
                EmployeeTypesDetail employeeTypesDetail = new();
                employeeTypesDetail.TypeName = type.Type;
                employeeTypesDetail.TotalEmployess = _unitOfWork.BandLocationLeaderPeriodEmployees.GetNumberOfEmployeeTypeInLocation(location.Id, type.Id, businessID);
                employeeTypesDetails.Add(employeeTypesDetail);
            }

            globalAccountWork.EmployeeTypesDetails = employeeTypesDetails;

            return globalAccountWork;
        }
        // Need Some Date? ::
        private async Task<GlobalAccountWork> AccountWorkLocationBandHelper(BandLocation band, string businessID)
        {
            List<EmployeeTypesDetail> employeeTypesDetails = new();
            GlobalAccountWork globalAccountWork = new();

            globalAccountWork.NumberOfEmployees = _unitOfWork.BandLocationLeaderPeriodEmployees.GetNumberOfEmployeesInBand(band.Id, businessID);
            globalAccountWork.NumberOfLeaders = _unitOfWork.BandLocationLeaders.NumberOfLeadersBand(band.Id, businessID);

            var types = await _unitOfWork.BandLocationLeaderPeriodEmployees.GetEmployeeTypesBand(band.Id);
            var filterdList = new List<EmployeeType>();

            foreach (var type in types)
            {
                if (!filterdList.Contains(type))
                {
                    filterdList.Add(type);
                }
            }

            foreach (var type in filterdList)
            {
                EmployeeTypesDetail employeeTypesDetail = new();
                employeeTypesDetail.TypeName = type.Type;
                employeeTypesDetail.TotalEmployess = _unitOfWork.BandLocationLeaderPeriodEmployees.GetNumberOfEmployeesInBand(band.Id, businessID);
                employeeTypesDetails.Add(employeeTypesDetail);
            }

            globalAccountWork.EmployeeTypesDetails = employeeTypesDetails;

            return globalAccountWork;
        }
        private async Task<List<EmployeeTypesDetail>> GetTypesHelper(IEnumerable<EmployeeType> types)
        {
            List<EmployeeTypesDetail> employeeTypesDetails = new();
            foreach (var type in types)
            {
                int count = await _unitOfWork.Employees.CountAsync(e => e.TypeId == type.Id);
                if (count > 0) 
                {
                    EmployeeTypesDetail employeeTypesDetail = new EmployeeTypesDetail
                    {
                        TypeName = type.Type,
                        TotalEmployess = count
                    };
                    employeeTypesDetails.Add(employeeTypesDetail);
                }
                           
            }
            return employeeTypesDetails;
        }

        #endregion

        #region AccountMoney:

        private GlobalAccountMoney AccountMoneyHelper(List<GlobalAccountMoney> globalAccounts)
        {
            GlobalAccountMoney globalAccountMoney = new();

            globalAccountMoney.TotalExtracts = globalAccounts.Sum(e => e.TotalExtracts);
            globalAccountMoney.TotalIncomes = globalAccounts.Sum(e => e.TotalIncomes);
            globalAccountMoney.TotalPayied = globalAccounts.Sum(e => e.TotalPayied);
            globalAccountMoney.TotalSalaryOFEmployees = globalAccounts.Sum(e => e.TotalSalaryOFEmployees);
            globalAccountMoney.TotalSalaryOFLeaders = globalAccounts.Sum(e => e.TotalSalaryOFLeaders);
            globalAccountMoney.TotalTransictions = globalAccounts.Sum(e => e.TotalTransictions);
            globalAccountMoney.TotalBills = globalAccounts.Sum(e => e.TotalBills);
            globalAccountMoney.TotalWesteds = globalAccounts.Sum(e => e.TotalWesteds);
            globalAccountMoney.GlobalSalary = globalAccountMoney.TotalSalaryOFEmployees + globalAccountMoney.TotalSalaryOFLeaders;
            globalAccountMoney.RemainderSalary = globalAccountMoney.GlobalSalary - globalAccountMoney.TotalPayied + globalAccountMoney.TotalPayied;
            globalAccountMoney.RemainderIncome = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalIncomes;
            globalAccountMoney.Profit = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalBills - globalAccountMoney.GlobalSalary - globalAccountMoney.TotalWesteds;

            return globalAccountMoney;
        }
        private GlobalAccountMoney AccountMoneyCompany(int? companyId, string businessId)
        {
            GlobalAccountMoney globalAccountMoney = new();
            globalAccountMoney.TotalBills = _unitOfWork.Bills.GetCompanyGlobalReport(companyId, businessId);
            globalAccountMoney.TotalExtracts = _unitOfWork.Extracts.GetCompanyGlobalReport(companyId, businessId);
            globalAccountMoney.TotalIncomes = _unitOfWork.Incomes.GetCompanyGlobalReport(companyId, businessId);
            globalAccountMoney.TotalPayied = _unitOfWork.Attendances.GetCompanyGlobalReportPaied(companyId, businessId)
                + _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetCompanyGlobalReportPaied(companyId, businessId);
            globalAccountMoney.TotalSalaryOFEmployees = _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetCompanyGlobalReportSalary(companyId, businessId);
            globalAccountMoney.TotalSalaryOFLeaders = _unitOfWork.Attendances.GetCompanyGlobalReportSalary(companyId, businessId);
            globalAccountMoney.TotalWesteds = _unitOfWork.BLWesteds.GetCompanyGlobalReport(companyId, businessId) + _unitOfWork.LeaderWesteds.GetCompanyGlobalReport(companyId,businessId);
            globalAccountMoney.TotalTransictions = _unitOfWork.LeaderTransactions.GetCompanyGlobalReport(companyId, businessId);
            globalAccountMoney.GlobalSalary = globalAccountMoney.TotalSalaryOFEmployees + globalAccountMoney.TotalSalaryOFLeaders;
            globalAccountMoney.RemainderIncome = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalIncomes;
            globalAccountMoney.RemainderSalary = globalAccountMoney.GlobalSalary - globalAccountMoney.TotalPayied;
            globalAccountMoney.Profit = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalWesteds - globalAccountMoney.TotalBills - globalAccountMoney.GlobalSalary;
            return globalAccountMoney;
        }
        private GlobalAccountMoney AccountMoneyBand(int? bandId, string businessId)
        {
            GlobalAccountMoney globalAccountMoney = new();
            globalAccountMoney.TotalBills = _unitOfWork.Bills.GetBandGlobalReport(bandId, businessId);
            globalAccountMoney.TotalExtracts = _unitOfWork.Extracts.GetBandGlobalReport(bandId, businessId);
            globalAccountMoney.TotalIncomes = _unitOfWork.Incomes.GetBandGlobalReport(bandId, businessId);
            globalAccountMoney.TotalPayied = _unitOfWork.Attendances.GetBandGlobalReportPaied(bandId, businessId) 
                + _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetBandGlobalReportPaied(bandId, businessId);
            globalAccountMoney.TotalSalaryOFEmployees = _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetBandGlobalReportSalary(bandId, businessId);
            globalAccountMoney.TotalSalaryOFLeaders = _unitOfWork.Attendances.GetBandGlobalReportSalary(bandId, businessId);
            globalAccountMoney.TotalWesteds = _unitOfWork.BLWesteds.GetBandGlobalReport(bandId, businessId) + _unitOfWork.LeaderWesteds.GetBandGlobalReport(bandId, businessId);
            globalAccountMoney.TotalTransictions = _unitOfWork.LeaderTransactions.GetBandGlobalReport(bandId, businessId);
            globalAccountMoney.GlobalSalary = globalAccountMoney.TotalSalaryOFEmployees + globalAccountMoney.TotalSalaryOFLeaders;
            globalAccountMoney.RemainderIncome = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalIncomes;
            globalAccountMoney.RemainderSalary = globalAccountMoney.GlobalSalary - globalAccountMoney.TotalPayied;
            globalAccountMoney.Profit = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalWesteds - globalAccountMoney.TotalBills - globalAccountMoney.GlobalSalary;

            return globalAccountMoney;
        }
        private GlobalAccountMoney AccountMoneyLocation(int? locationId, string businessId)
        {
            GlobalAccountMoney globalAccountMoney = new();
            globalAccountMoney.TotalBills = _unitOfWork.Bills.GetLocationGlobalReport(locationId, businessId);
            globalAccountMoney.TotalExtracts = _unitOfWork.Extracts.GetLocationGlobalReport(locationId, businessId);
            globalAccountMoney.TotalIncomes = _unitOfWork.Incomes.GetLocationGlobalReport(locationId, businessId);
            globalAccountMoney.TotalPayied = _unitOfWork.Attendances.GetBandGlobalReportPaied(locationId, businessId)
                + _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetLocationGlobalReportPaied(locationId, businessId);
            globalAccountMoney.TotalSalaryOFEmployees = _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetLocationGlobalReportSalary(locationId, businessId);
            globalAccountMoney.TotalSalaryOFLeaders = _unitOfWork.Attendances.GetLocationGlobalReportSalary(locationId, businessId);
            globalAccountMoney.TotalWesteds = _unitOfWork.BLWesteds.GetLocationGlobalReport(locationId, businessId) + _unitOfWork.LeaderWesteds.GetLocationGlobalReport(locationId, businessId);
            globalAccountMoney.TotalTransictions = _unitOfWork.LeaderTransactions.GetTotalLocation(locationId);
            globalAccountMoney.GlobalSalary = globalAccountMoney.TotalSalaryOFEmployees + globalAccountMoney.TotalSalaryOFLeaders;
            globalAccountMoney.RemainderIncome = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalIncomes;
            globalAccountMoney.RemainderSalary = globalAccountMoney.GlobalSalary - globalAccountMoney.TotalPayied;
            globalAccountMoney.Profit = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalWesteds - globalAccountMoney.TotalBills - globalAccountMoney.GlobalSalary;

            return globalAccountMoney;
        }

        #endregion

        
    }
}

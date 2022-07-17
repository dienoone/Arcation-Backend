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
            var entities = await _unitOfWork.Companies.GlobalReport(businessId);
            var Ids = entities.Select(e => e.Id);
            List<GlobalAccountMoney> preGlobalAccountMoney = new();
            foreach (int id in Ids)
            {
                preGlobalAccountMoney.Add(AccountMoneyCompany(id));
            }
            GlobalReport globalReport = new();

            GlobalAccount globalAccount = new();

            GlobalAccountWork globalAccountWork = await AccountWorkHelper(businessId);
            GlobalAccountMoney globalAccountMoney = AccountMoneyHelper(preGlobalAccountMoney);
            globalAccount.GlobalAccountMoney = globalAccountMoney;
            globalAccount.GlobalAccountWork = globalAccountWork;

            List<CompanyReprots> companyReprots = _mapper.Map<List<CompanyReprots>>(entities);

            globalReport.Companies = companyReprots;
            globalReport.globalAccount = globalAccount;

            return Ok(globalReport);
        }

        // api/GlobalReports/{companyId}
        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetSingleCompanyReport([FromRoute] int? companyId)
        {
            if (companyId != null)
            {
                GlobalReport globalReport = new();
                GlobalAccount globalAccount = new();
                var company = await _unitOfWork.Companies.SingleGlobalReport(HttpContext.GetBusinessId(), companyId);

                globalAccount.GlobalAccountWork = await AccountWorkCompanyHelper(company);
                globalAccount.GlobalAccountMoney = AccountMoneyCompany(companyId);

                var locations = company.Locations;
                globalReport.Locations = _mapper.Map<IEnumerable<CompanyLocationsReports>>(locations);
                globalReport.globalAccount = globalAccount ;

                return Ok(globalReport);
            }
            return NotFound();
        }

        // api/GlobalReports/Company/{locationId}
        [HttpGet("Company/{locationId}")]
        public async Task<IActionResult> GetSingleLocationReport([FromRoute] int? locationId)
        {
            if (locationId != null)
            {
                var location = await _unitOfWork.Locations.GetLocationReport(locationId, HttpContext.GetBusinessId());
                if (location != null)
                {
                    GlobalReport globalReport = new();
                    GlobalAccount globalAccount = new();
                    var preBands = location.BandLocations;

                    globalAccount.GlobalAccountMoney = AccountMoneyLocation(locationId);
                    globalAccount.GlobalAccountWork = await AccountWorkLocationHelper(location);
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
                var band = await _unitOfWork.BandLocations.GetBandLocationReport(bandLocationId, HttpContext.GetBusinessId());
                if (band != null)
                {
                    GlobalReport globalReport = new();
                    GlobalAccount globalAccount = new();

                    globalAccount.GlobalAccountMoney = AccountMoneyBand(bandLocationId);
                    globalAccount.GlobalAccountWork = await AccountWorkLocationBandHelper(band);

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
        private async Task<GlobalAccountWork> AccountWorkCompanyHelper(Company company)
        {
            GlobalAccountWork globalAccountWork = new();
            List<EmployeeTypesDetail> employeeTypesDetails = new();

            globalAccountWork.CompanyName = company.Name;
            globalAccountWork.NumberOfBands = company.Locations.Sum(e => e.BandLocations.Count);
            globalAccountWork.NumberOfEmployees = company.Locations.Sum(e => e.BandLocations.Sum(e => e.BandLocationLeaders.Sum(e => e.BandLocationLeaderPeriods.Sum(e => e.BandLocationLeaderPeriodEmployees.Count))));
            globalAccountWork.NumberOfLeaders = company.Locations.Sum(e => e.BandLocations.Sum(e => e.BandLocationLeaders.Count));
            globalAccountWork.NumberOfLocations = company.Locations.Count;

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
                employeeTypesDetail.TotalEmployess = company.Locations.Sum(e => e.BandLocations.Sum(e => e.BandLocationLeaders.Sum(e => e.BandLocationLeaderPeriods.Sum(e => e.BandLocationLeaderPeriodEmployees.Count(e => e.Employee.Type.Type == type.Type)))));
                employeeTypesDetails.Add(employeeTypesDetail);
            }

            globalAccountWork.EmployeeTypesDetails = employeeTypesDetails;

            return globalAccountWork;
        }
        private async Task<GlobalAccountWork> AccountWorkLocationHelper(Location location)
        {
            List<EmployeeTypesDetail> employeeTypesDetails = new();
            GlobalAccountWork globalAccountWork = new();

            globalAccountWork.NumberOfBands = location.BandLocations.Count;
            globalAccountWork.NumberOfEmployees = location.BandLocations.Sum(e => e.BandLocationLeaders.Sum(e => e.BandLocationLeaderPeriods.Sum(e => e.BandLocationLeaderPeriodEmployees.Count)));
            globalAccountWork.NumberOfLeaders = location.BandLocations.Sum(e => e.BandLocationLeaders.Count);
            globalAccountWork.StartingDate = location.StartingDate;
            globalAccountWork.EndingDate = location.EndingDate;
            globalAccountWork.CompanyName = location.Company.Name;

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
                employeeTypesDetail.TotalEmployess = location.BandLocations.Sum(e => e.BandLocationLeaders.Sum(e => e.BandLocationLeaderPeriods.Sum(e => e.BandLocationLeaderPeriodEmployees.Count(e => e.Employee.Type.Type == type.Type))));
                employeeTypesDetails.Add(employeeTypesDetail);
            }

            globalAccountWork.EmployeeTypesDetails = employeeTypesDetails;

            return globalAccountWork;
        }
        // Need Some Date? ::
        private async Task<GlobalAccountWork> AccountWorkLocationBandHelper(BandLocation band)
        {
            List<EmployeeTypesDetail> employeeTypesDetails = new();
            GlobalAccountWork globalAccountWork = new();

            globalAccountWork.NumberOfEmployees = band.BandLocationLeaders.Sum(e => e.BandLocationLeaderPeriods.Sum(e => e.BandLocationLeaderPeriodEmployees.Count));
            globalAccountWork.NumberOfLeaders = band.BandLocationLeaders.Count;

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
                employeeTypesDetail.TotalEmployess = band.BandLocationLeaders.Sum(e => e.BandLocationLeaderPeriods.Sum(e => e.BandLocationLeaderPeriodEmployees.Count(e => e.Employee.Type.Type == type.Type)));
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
                EmployeeTypesDetail employeeTypesDetail = new EmployeeTypesDetail
                {
                    TypeName = type.Type,
                    TotalEmployess = await _unitOfWork.Employees.CountAsync(e => e.TypeId == type.Id)
                };
                employeeTypesDetails.Add(employeeTypesDetail);
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
        private GlobalAccountMoney AccountMoneyCompany(int? companyId)
        {
            GlobalAccountMoney globalAccountMoney = new();
            globalAccountMoney.TotalBills = _unitOfWork.Bills.GetTotalCompany(companyId);
            globalAccountMoney.TotalExtracts = _unitOfWork.ExtractRows.GetTotalCompany(companyId);
            globalAccountMoney.TotalIncomes = _unitOfWork.Incomes.GetTotalCompany(companyId);
            globalAccountMoney.TotalPayied = _unitOfWork.BandLocationLeaderPeriods.GetTotalCompanyPaied(companyId) + _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetTotalCompanyPaied(companyId) + _unitOfWork.Attendances.GetTotalCompanyBorrow(companyId) + _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetTotalCompanyBorrow(companyId);
            globalAccountMoney.TotalSalaryOFEmployees = _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetTotalCompanyHours(companyId) * _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetTotalCompanySalary(companyId);
            globalAccountMoney.TotalSalaryOFLeaders = _unitOfWork.Attendances.GetTotalCompanyHours(companyId) * _unitOfWork.BandLocationLeaderPeriods.GetTotalCompanySalary(companyId);
            globalAccountMoney.TotalWesteds = _unitOfWork.BLWesteds.GetTotalCompany(companyId) + _unitOfWork.LeaderWesteds.GetTotalCompany(companyId);
            globalAccountMoney.TotalTransictions = _unitOfWork.LeaderTransactions.GetTotalCompany(companyId);
            globalAccountMoney.GlobalSalary = globalAccountMoney.TotalSalaryOFEmployees + globalAccountMoney.TotalSalaryOFLeaders;
            globalAccountMoney.RemainderIncome = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalIncomes;
            globalAccountMoney.RemainderSalary = globalAccountMoney.GlobalSalary - globalAccountMoney.TotalPayied;
            globalAccountMoney.Profit = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalWesteds - globalAccountMoney.TotalBills - globalAccountMoney.GlobalSalary;
            return globalAccountMoney;
        }
        private GlobalAccountMoney AccountMoneyBand(int? bandId)
        {
            GlobalAccountMoney globalAccountMoney = new();
            globalAccountMoney.TotalBills = _unitOfWork.Bills.GetTotalBand(bandId);
            globalAccountMoney.TotalExtracts = _unitOfWork.ExtractRows.GetTotalBand(bandId);
            globalAccountMoney.TotalIncomes = _unitOfWork.Incomes.GetTotalBand(bandId);
            globalAccountMoney.TotalPayied = _unitOfWork.BandLocationLeaderPeriods.GetTotalBandPaied(bandId) + _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetTotalBandPaied(bandId) + _unitOfWork.Attendances.GetTotalBandBorrow(bandId) + _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetTotalBandBorrow(bandId);
            globalAccountMoney.TotalSalaryOFEmployees = _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetTotalBandHours(bandId) * _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetTotalBandSalary(bandId);
            globalAccountMoney.TotalSalaryOFLeaders = _unitOfWork.Attendances.GetTotalBandHours(bandId) * _unitOfWork.BandLocationLeaderPeriods.GetTotalBandSalary(bandId);
            globalAccountMoney.TotalWesteds = _unitOfWork.BLWesteds.GetTotalBand(bandId) + _unitOfWork.LeaderWesteds.GetTotalBand(bandId);
            globalAccountMoney.TotalTransictions = _unitOfWork.LeaderTransactions.GetTotalBand(bandId);
            globalAccountMoney.GlobalSalary = globalAccountMoney.TotalSalaryOFEmployees + globalAccountMoney.TotalSalaryOFLeaders;
            globalAccountMoney.RemainderIncome = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalIncomes;
            globalAccountMoney.RemainderSalary = globalAccountMoney.GlobalSalary - globalAccountMoney.TotalPayied;
            globalAccountMoney.Profit = globalAccountMoney.TotalExtracts - globalAccountMoney.TotalWesteds - globalAccountMoney.TotalBills - globalAccountMoney.GlobalSalary;

            return globalAccountMoney;
        }
        private GlobalAccountMoney AccountMoneyLocation(int? locationId)
        {
            GlobalAccountMoney globalAccountMoney = new();
            globalAccountMoney.TotalBills = _unitOfWork.Bills.GetTotalLocation(locationId);
            globalAccountMoney.TotalExtracts = _unitOfWork.ExtractRows.GetTotalLocation(locationId);
            globalAccountMoney.TotalIncomes = _unitOfWork.Incomes.GetTotalLocation(locationId);
            globalAccountMoney.TotalPayied = _unitOfWork.BandLocationLeaderPeriods.GetTotalLocationPaied(locationId) + _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetTotalLocationPaied(locationId) + _unitOfWork.Attendances.GetTotalLocationBorrow(locationId) + _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetTotalLocationBorrow(locationId);
            globalAccountMoney.TotalSalaryOFEmployees = _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetTotalLocationHours(locationId) * _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetTotalLocationSalary(locationId);
            globalAccountMoney.TotalSalaryOFLeaders = _unitOfWork.Attendances.GetTotalLocationHours(locationId) * _unitOfWork.BandLocationLeaderPeriods.GetTotalLocationSalary(locationId);
            globalAccountMoney.TotalWesteds = _unitOfWork.BLWesteds.GetTotalLocation(locationId) + _unitOfWork.LeaderWesteds.GetTotalLocation(locationId);
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

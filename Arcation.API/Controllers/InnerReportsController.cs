using Arcation.Core;
using Arcation.Core.ViewModels.ArcationViewModel;
using Arcation.API.Extentions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcation.Core.Models.ArcationModels.Main;

namespace Arcation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InnerReportsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        // bandLocationId : 

        public InnerReportsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // api/InnerReports/{bandLocationId}
        [HttpGet("{bandLocationId}")]
        public async Task<IActionResult> GetReports([FromRoute] int? bandLocationId)
        {
            if(bandLocationId != null)
            {
                string businessId = HttpContext.GetBusinessId();
                BandLocation bandLocation = await _unitOfWork.BandLocations.FindAsync(e => e.Id == bandLocationId && e.BusinessId == businessId && !e.IsDeleted);
                if(bandLocation != null)
                {                   
                    BLReport bLReport = new BLReport();
                    BLCompanyReport bLCompanyReport = new();
                    BLLeadersReport bLLeadersReport = new();
                    EmployeesDetail employeesDetail = new();

                    //var entity = await _unitOfWork.BandLocations.Reports(bandLocationId, businessId);

                    // ----- Employee Types:
                    var employeesTypes = await _unitOfWork.EmployeeTypes.FindAllAsync(e => e.BusinessId == businessId);
                    List<EmployeeTypesDetail> employeeTypesDetails = new List<EmployeeTypesDetail>();
                    foreach (var employeesType in employeesTypes)
                    {

                        int count = _unitOfWork.BandLocationLeaderPeriodEmployees.GetBandLocationInnerReport(bandLocationId, employeesType.Id, businessId);
                        if (count > 0)
                        {
                            EmployeeTypesDetail detail = new EmployeeTypesDetail
                            {
                                TypeName = employeesType.Type,
                                TotalEmployess = count
                            };
                            employeeTypesDetails.Add(detail);
                        }
                    }


                    bLCompanyReport.TotalBills = _unitOfWork.Bills.GetBandLocationInnerReport(bandLocationId, businessId);
                    bLCompanyReport.TotalExtracts = _unitOfWork.Extracts.GetBandLocationInnerReport(bandLocationId, businessId);
                    bLCompanyReport.TotalIncomes = _unitOfWork.Incomes.GetBandLocationInnerReport(bandLocationId, businessId);
                    bLCompanyReport.TotalWested = _unitOfWork.BLWesteds.GetBandLocationInnerReport(bandLocationId, businessId);
                    bLCompanyReport.TotalSalaryOfEmployees = _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetBandLocationInnerReport(bandLocationId, businessId)
                        + _unitOfWork.Attendances.GetBandLocationInnerReport(bandLocationId, businessId);
                    bLCompanyReport.Remainder = bLCompanyReport.TotalExtracts - bLCompanyReport.TotalIncomes;

                    bLLeadersReport.TotalTransictions = _unitOfWork.LeaderTransactions.GetBandLocationInnerReport(bandLocationId, businessId);
                    bLLeadersReport.TotalWesteds = _unitOfWork.LeaderWesteds.GetBandLocationInnerReport(bandLocationId, businessId);
                    bLLeadersReport.TotalSalaryOfLeaders = _unitOfWork.Attendances.GetBandLocationInnerReport(bandLocationId, businessId);
                    bLLeadersReport.TotalPaied = _unitOfWork.Attendances.GetBandLocationInnerReportPaied(bandLocationId, businessId);
                    bLLeadersReport.Remainder = bLLeadersReport.TotalSalaryOfLeaders - bLLeadersReport.TotalPaied;


                    employeesDetail.TotalSalaryOfEmployess = _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetBandLocationInnerReport(bandLocationId, businessId);
                    employeesDetail.TotalPaied = _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetBandLocationInnerReport(bandLocationId, businessId) 
                        + _unitOfWork.BandLocationLeaderPeriodEmployeePeriodAttendances.GetBandLocationInnerReportBorrow(bandLocationId, businessId);
                    employeesDetail.TotalRemainder = employeesDetail.TotalSalaryOfEmployess - employeesDetail.TotalPaied;
                    employeesDetail.employeeTypesDetails = employeeTypesDetails;

                    bLReport.CompanyReport = bLCompanyReport;
                    bLReport.LLeadersReport = bLLeadersReport;
                    bLReport.EmployeesReport = employeesDetail;

                    return Ok(bLReport);
                }
                return NotFound();
            }
            return BadRequest();
        }
    }
}

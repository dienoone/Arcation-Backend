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
                
                var entity = await _unitOfWork.BandLocations.Reports(bandLocationId, HttpContext.GetBusinessId());

                // ----- Employee Types:
                var employeesTypes = await _unitOfWork.EmployeeTypes.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId());
                List<EmployeeTypesDetail> employeeTypesDetails = new List<EmployeeTypesDetail>();
                foreach(var employeesType in employeesTypes)
                {
                    var employees = entity.BandLocationLeaders.Select(e => e.BandLocationLeaderPeriods.Select(e => e.BandLocationLeaderPeriodEmployees.Where(e => e.Employee.TypeId == employeesType.Id)));
                    if(employees.Count() > 0)
                    {
                        EmployeeTypesDetail detail = new EmployeeTypesDetail
                        {
                            TypeName = employeesType.Type,
                            TotalEmployess = employees.Count()
                        };
                        employeeTypesDetails.Add(detail);
                    }
                }

                BLReport bLReport = new BLReport();
                
                BLCompanyReport bLCompanyReport = _mapper.Map<BLCompanyReport>(entity);
                bLCompanyReport.Remainder = bLCompanyReport.TotalExtracts - bLCompanyReport.TotalIncomes;
                BLLeadersReport bLLeadersReport = _mapper.Map<BLLeadersReport>(entity);
                bLLeadersReport.Remainder = bLLeadersReport.TotalSalaryOfLeaders - bLLeadersReport.TotalPaied;
                EmployeesDetail employeesDetail = _mapper.Map<EmployeesDetail>(entity);
                employeesDetail.TotalRemainder = employeesDetail.TotalSalaryOfEmployess - employeesDetail.TotalPaied;
                employeesDetail.employeeTypesDetails = employeeTypesDetails;

                bLReport.CompanyReport = bLCompanyReport;
                bLReport.LLeadersReport = bLLeadersReport;
                bLReport.EmployeesReport = employeesDetail;

                return Ok(bLReport);

            }
            return BadRequest();
        }
    }
}

using Arcation.API.Extentions;
using Arcation.API.Handler;
using Arcation.Core;
using Arcation.Core.Models;
using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize(Roles = "Admin, User")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageHandler _imageHandler;
        private readonly AppURL _appURL;
        // Employees Page Here When Admin needed To Finish Employee: 
        // And Add Employee:
        // Delete Old Photos : Neeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeded
        public EmployeesController(IMapper mapper, IUnitOfWork unitOfWork, IImageHandler imageHandler, IOptions<AppURL> appUrl)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _imageHandler = imageHandler;
            _appURL = appUrl.Value;
        }

        #region Employee Page:

        // api/employees
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Employee> entities = await _unitOfWork.Employees.GetAllIncludeTypes(HttpContext.GetBusinessId());
            if (entities != null)
            {
                return Ok(_mapper.Map<IEnumerable<EmployeePageDto>>(entities));
            }
            return NoContent();
        }

        // api/employees/{Id} => Get Single Employee:
        [HttpGet("{Id}", Name = "GetEmployee")]
        public async Task<IActionResult> GetEmployee([FromRoute] int? Id)
        {
            if (Id != null)
            {
                Employee entity = await _unitOfWork.Employees.GetEmployeeIncludeTypes(Id, HttpContext.GetBusinessId());
                if (entity != null)
                {
                    return Ok(_mapper.Map<EmployeeDetailsDto>(entity));
                }
                return NotFound();
            }
            return NotFound();

        }

        #endregion

        #region Employee Details :

        #region SubPeriods:

        // api/employees/employeePeriods/SubPeriod/{bandLocationLeaderPeriodEmployeePeriodId} :
        [HttpGet("employeePeriods/SubPeriod/{bandLocationLeaderPeriodEmployeePeriodId}")]
        public async Task<IActionResult> GetSubPeriods(int? bandLocationLeaderPeriodEmployeePeriodId)
        {
            if (bandLocationLeaderPeriodEmployeePeriodId != null)
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
        // api/employees/finishSubPeriod/{bandLocationLeaderPeriodEmployeePeriodId}
        [HttpPost("finishSubPeriod/{bandLocationLeaderPeriodEmployeePeriodId}")]
        public async Task<IActionResult> FinishSubPeriod([FromRoute] int? bandLocationLeaderPeriodEmployeePeriodId, [FromBody] FinishSupPeriodAdminDto dto)
        {
            if (bandLocationLeaderPeriodEmployeePeriodId != null)
            {
                BandLocationLeaderPeriodEmployeePeriod bandLocationLeaderPeriodEmployeePeriod = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetSubPeriodIncludeAttendace(bandLocationLeaderPeriodEmployeePeriodId, HttpContext.GetBusinessId());
                if (bandLocationLeaderPeriodEmployeePeriod != null)
                {
                    double totalDays = 0;
                    double borrowValue = 0;
                    foreach (var attendance in bandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployeePeriodAttendances)
                    {
                        totalDays += attendance.WorkingHours;
                        borrowValue += attendance.BorrowValue;
                    }

                    double employeeSalary = totalDays * bandLocationLeaderPeriodEmployeePeriod.EmployeeSalary;
                    double totalPaied = bandLocationLeaderPeriodEmployeePeriod.PayiedValue + dto.PayiedValue;
                    double remainder = employeeSalary - totalPaied - borrowValue;
                    if (remainder < 0)
                    {
                        return BadRequest();
                    }

                    bandLocationLeaderPeriodEmployeePeriod.PayiedValue += dto.PayiedValue;
                    if (bandLocationLeaderPeriodEmployeePeriod.PayiedValue == employeeSalary)
                    {
                        bandLocationLeaderPeriodEmployeePeriod.State = true;
                    }

                    if (await _unitOfWork.Complete())
                    {
                        BandLocationLeaderPeriodEmployeePeriod subPeriod = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetSubPeriodIncludeAttendace(bandLocationLeaderPeriodEmployeePeriod.Id, HttpContext.GetBusinessId());
                        if (subPeriod != null)
                        {
                            SubPeriodDetailDto subPeriodDetailDto = _mapper.Map<SubPeriodDetailDto>(subPeriod);
                            subPeriodDetailDto.Remainder = subPeriodDetailDto.TotalSalary - subPeriodDetailDto.TotalPaied - subPeriodDetailDto.TotalBorrow;
                            return Ok(subPeriodDetailDto);
                        }
                        return NotFound();
                    }
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/employees/EditSubPeriod/{SubPeriodId}
        [HttpPut("EditSubPeriod/{SubPeriodId}")]
        public async Task<IActionResult> EditSubPeriod([FromRoute] int? SubPeriodId, [FromBody] UpdateSubPeriod dto)
        {
            if (SubPeriodId != null)
            {
                if (ModelState.IsValid)
                {
                    BandLocationLeaderPeriodEmployeePeriod subPeriod = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.FindAsync(e => e.Id == SubPeriodId);
                    if (subPeriod != null)
                    {
                        subPeriod.EmployeeSalary = dto.EmployeeSalary;
                        if (await _unitOfWork.Complete())
                        {
                            BandLocationLeaderPeriodEmployeePeriod subPeriodDto = await _unitOfWork.BandLocationLeaderPeriodEmployeePeriods.GetSubPeriodIncludeAttendace(SubPeriodId, HttpContext.GetBusinessId());
                            if (subPeriodDto != null)
                            {
                                SubPeriodDetailDto subPeriodDetailDto = _mapper.Map<SubPeriodDetailDto>(subPeriod);
                                subPeriodDetailDto.Remainder = subPeriodDetailDto.TotalSalary - subPeriodDetailDto.TotalPaied - subPeriodDetailDto.TotalBorrow;
                                return Ok(subPeriodDetailDto);
                            }
                            return NotFound();
                        }

                    }
                    return NotFound();
                }
                return BadRequest(ModelState);
            }
            return NotFound();
        }

        #endregion

        // api/employees/employeeDetails/{Id}
        [HttpGet("employeeDetails/{Id}")]
        public async Task<IActionResult> GetEmployeeWithLocations([FromRoute] int? Id)
        {
            if (Id != null)
            {
                Employee queryEmployee = await _unitOfWork.Employees.GetEmployeePeriods(Id, HttpContext.GetBusinessId());
                if (queryEmployee != null)
                {
                    var Locations = await _unitOfWork.Locations.LocationRelatedToEmployee(Id, HttpContext.GetBusinessId());
                    EmployeeWithLocationsDto employeeWithLocationsDto = new();
                    IEnumerable<LocationNames> locationNames = _mapper.Map<IEnumerable<LocationNames>>(Locations);

                    EmployeeDetailsDto employeeDetailsDto = _mapper.Map<EmployeeDetailsDto>(queryEmployee);
                    EmployeeBusinessDetailDto employeeBusinessDetailDto = _mapper.Map<EmployeeBusinessDetailDto>(queryEmployee);
                    employeeBusinessDetailDto.TotalRemainder = employeeBusinessDetailDto.TotalSalary - employeeBusinessDetailDto.TotalPayied;
                    employeeWithLocationsDto.EmployeeBusiness = employeeBusinessDetailDto;
                    employeeWithLocationsDto.EmployeeDetail = employeeDetailsDto;
                    employeeWithLocationsDto.locationNames = locationNames;

                    return Ok(employeeWithLocationsDto);
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/employees/employeeDetails/{employeeID}/Periods/{locationID}
        [HttpGet("employeeDetails/{employeeID}/Periods/{locationID}")]
        public async Task<IActionResult> GetEmployeeSubPeriods([FromRoute] int? locationID, [FromRoute] int? employeeID)
        {
            if (locationID != null && employeeID != null)
            {
                var period = await _unitOfWork.BandLocationLeaderPeriodEmployees.GetPeriodDetailsAsync(locationID, employeeID, HttpContext.GetBusinessId());
                if (period != null)
                {
                    EmployeeMainPeriodDetailPageDto employeeMainPeriodDetailPageDto = _mapper.Map<EmployeeMainPeriodDetailPageDto>(period);
                    employeeMainPeriodDetailPageDto.Remainder = employeeMainPeriodDetailPageDto.TotalSalary - employeeMainPeriodDetailPageDto.TotalPaied - employeeMainPeriodDetailPageDto.TotalBorrow;
                    return Ok(employeeMainPeriodDetailPageDto);
                }
                return NotFound();
            }
            return NotFound();
        }

        #endregion

        // api/employees => create employee
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] AddEmployeeDto dto)
        {
            if (ModelState.IsValid)
            {
                Employee isExist = await _unitOfWork.Employees.FindAsync(e => e.Name == dto.Name && e.BusinessId == HttpContext.GetBusinessId());
                if (isExist == null)
                {
                    Employee newEmployee = new Employee
                    {
                        Name = dto.Name,
                        Phone = dto.Phone,
                        Salary = dto.Salary,
                        TypeId = dto.TypeId,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = HttpContext.GetUserId(),
                        BusinessId = HttpContext.GetBusinessId()
                    };

                    // ----- This Part Related To Photos ---------
                    string photo = "photo";
                    string IdentityPhoto = "IdentityPhoto";
                    if (dto.Photo != null)
                    {
                        if (dto.Photo.Length > 0)
                        {
                            photo = await _imageHandler.UploadImage(dto.Photo);
                            if (photo == "Invalid image file") return BadRequest();
                        }
                    }
                    if (dto.IdentityPhoto != null)
                    {
                        if (dto.IdentityPhoto.Length > 0)
                        {
                            IdentityPhoto = await _imageHandler.UploadImage(dto.IdentityPhoto);
                            if (IdentityPhoto == "Invalid image file") return BadRequest();
                        }
                    }
                    if (photo != "photo")
                    {
                        newEmployee.Photo = _appURL.AppUrl + photo;
                    }
                    else
                    {
                        newEmployee.Photo = null;
                    }
                    if (IdentityPhoto != "IdentityPhoto")
                    {
                        newEmployee.IdentityPhoto = _appURL.AppUrl + IdentityPhoto;
                    }
                    else
                    {
                        newEmployee.IdentityPhoto = null;
                    }
                    // --------------- End Region ----------------

                    var result = await _unitOfWork.Employees.AddAsync(newEmployee);

                    if (result != null)
                    {
                        if (await _unitOfWork.Complete())
                        {
                            var getEmployee = await _unitOfWork.Employees.GetEmployeeIncludeTypes(newEmployee.Id, HttpContext.GetBusinessId());
                            return CreatedAtRoute("GetEmployee", new { controller = "Employees", id = newEmployee.Id }, _mapper.Map<EmployeeDetailsDto>(getEmployee));
                        }
                        return BadRequest();
                    }
                    return BadRequest();

                }
                return BadRequest("هذا الاسم موجود بالفعل");
            }
            return BadRequest(ModelState);
        }

        // api/employees/{id} => Update Employee
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int? id, [FromBody] UpdateEmployeeDto dto)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    Employee isNameExist = await _unitOfWork.Employees.FindAsync(e => e.Name == dto.Name && e.BusinessId == HttpContext.GetBusinessId());
                    if (isNameExist == null)
                    {
                        Employee queryEmployee = await _unitOfWork.Employees.FindAsync(b => b.Id == id && b.BusinessId == HttpContext.GetUserId());
                        if (queryEmployee != null)
                        {
                            queryEmployee.Name = dto.Name;
                            queryEmployee.Phone = dto.Phone;
                            queryEmployee.Salary = dto.Salary;
                            queryEmployee.TypeId = dto.TypeId;

                            // ----- This Part Related To Photos ---------
                            string photo = "photo";
                            string IdentityPhoto = "IdentityPhoto";
                            if (dto.Photo != null)
                            {
                                if (dto.Photo.Length > 0)
                                {
                                    photo = await _imageHandler.UploadImage(dto.Photo);
                                    if (photo == "Invalid image file") return BadRequest();
                                }
                            }
                            if (dto.IdentityPhoto != null)
                            {
                                if (dto.IdentityPhoto.Length > 0)
                                {
                                    IdentityPhoto = await _imageHandler.UploadImage(dto.IdentityPhoto);
                                    if (IdentityPhoto == "Invalid image file") return BadRequest();
                                }
                            }
                            if (photo != "photo")
                            {
                                queryEmployee.Photo = _appURL.AppUrl + photo;
                            }
                            else
                            {
                                queryEmployee.Photo = null;
                            }
                            if (IdentityPhoto != "IdentityPhoto")
                            {
                                queryEmployee.IdentityPhoto = _appURL.AppUrl + IdentityPhoto;
                            }
                            else
                            {
                                queryEmployee.IdentityPhoto = null;
                            }
                            // --------------- End Region ----------------

                            if (await _unitOfWork.Complete())
                            {
                                var getEmployee = await _unitOfWork.Employees.GetEmployeeIncludeTypes(queryEmployee.Id, HttpContext.GetBusinessId());
                                return Ok(_mapper.Map<EmployeeDetailsDto>(getEmployee));
                            }
                            return BadRequest();
                        }
                        return NotFound();
                    }
                    return BadRequest("هذا الاسم موجود بالفعل");
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/employees/{id} => Delete Employee
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int? id)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    Employee queryEmployee = await _unitOfWork.Employees.FindAsync(b => b.Id == id && b.BusinessId == HttpContext.GetBusinessId());
                    if (queryEmployee != null)
                    {
                        queryEmployee.IsDeleted = true;
                        if (await _unitOfWork.Complete())
                        {
                            return NoContent();
                        }
                        return BadRequest();
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        #region Helper: BandLocationEmployeesController

        // api/employees/addRequire 

        [HttpGet("AddRequire")]
        public async Task<IActionResult> GetAddRequire()
        {
            IEnumerable<Employee> entities = await _unitOfWork.Employees.FindAllAsync(l => !l.IsDeleted && l.BusinessId == HttpContext.GetBusinessId(), new string[] { "Type" });
            return Ok(_mapper.Map<IEnumerable<AddEmployeeToPeriodRequireDto>>(entities));
        }

        #endregion
    }
}

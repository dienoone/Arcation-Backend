using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class AddEmployeeDto
    {
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        [Required]
        public double Salary { get; set; }
        [Required]
        public int TypeId { get; set; }

        // Updates, Photos:
        public IFormFile Photo { get; set; }
        public IFormFile IdentityPhoto { get; set; }
    }
    public class UpdateEmployeeDto : AddEmployeeDto
    {
    }

    public class EmployeePageDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string Photo { get; set; }
    }
    public class EmployeeDetailsDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Salary { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Photo { get; set; }
        public string IdentityPhoto { get; set; }
    }
    // Need To Changed : EmployeeWithLocationsDto
    public class EmployeeWithLocationsDto
    {
        public EmployeeDetailsDto EmployeeDetail { get; set; }
        public EmployeeBusinessDetailDto EmployeeBusiness { get; set; }
        public IEnumerable<LocationNames> locationNames { get; set; }
    }

    public class LocationNames
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }

    }


    public class EmployeeBusinessDetailDto
    {
        public double TotalDays { get; set; }
        public double TotalSalary { get; set; }
        public double TotalPayied { get; set; }
        public double TotalRemainder { get; set; }


    }

    public class EmployeeMainPeriodDetailPageDto
    {
        public int BandLocationLeaderPeriodEmployeeId { get; set; }
        public string LocationName { get; set; }
        public double EmployeeSalary { get; set; }
        public double TotalDays { get; set; }
        public double TotalSalary { get; set; }
        public double TotalBorrow { get; set; }
        public double TotalPaied { get; set; }
        public double Remainder { get; set; }
        public bool state { get; set; }
        public List<EmployeeChildPeriodDetailPageDto> SubPeriods { get; set; }

    }

    public class EmployeeChildPeriodDetailPageDto
    {
        public int BandLocationLeaderPeriodEmployeePeriodId { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public bool State { get; set; }

    }

    public class SubPeriodDetailDto
    {
        public int BandLocationLeaderPeriodEmployeePeriodId { get; set; }
        public int MainPeriodId { get; set; }
        public string LeaderName { get; set; }
        public string BandName { get; set; }
        public string EmployeeType { get; set; }
        public double EmployeeSalary { get; set; }
        public double TotalDays { get; set; }
        public double TotalSalary { get; set; }
        public double TotalBorrow { get; set; }
        public double TotalPaied { get; set; }
        public double Remainder { get; set; }
        public bool state { get; set; }
    }

    public class AddEmployeeToPeriodRequireDto
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class FinishSupPeriodAdminDto
    {
        [Required]
        public double PayiedValue { get; set; }
    }

    public class FinishSupPeriodLeaderDto
    {
        [Required]
        public bool EmployeeState { get; set; }
    }

}

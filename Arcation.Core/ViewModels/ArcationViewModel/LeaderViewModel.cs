using Arcation.Core.Models.ArcationModels.Main;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class AddLeaderDto
    {
        [Required]
        public string FristName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }
        [Required]
        public double Salary { get; set; }

        [JsonIgnore]
        public string BusinessId { get; set; }

        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }

        // Updates, Photos:
        public IFormFile Photo { get; set; }
        public IFormFile IdentityPhoto { get; set; }
    }

    public class UpdateLeaderDto : AddLeaderDto { }

    public class LeaderInfoDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Salary { get; set; }
        public string UserName { get; set; }
        public string Passwrod { get; set; }
        public string Photo { get; set; }
        public string IdentityPhoto { get; set; }

    }

    public class LeadersPageDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
    }
    // ------------------------------------------------

    public class LeaderDetails
    {
        public LeaderInfoDto LeaderInfo { get; set; }
        public EmployeeBusinessDetailDto BusinessDetail { get; set; }
        public IEnumerable<LeaderLocations> LeaderLocations { get; set; }
    }
    public class LeaderLocations
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public List<LeaderLocationBands> BandLocations { get; set; }

    }
    public class LeaderLocationBands
    {
        public int BandLocationLeaderId { get; set; }
        public int BandId { get; set; }
        public string BandName { get; set; }
    }

    // -------------------------------------------------------

    public class LeaderLocationDetail
    {
        public string LocationName { get; set; }
        public string BandName { get; set; }
        public double LeaderSalary { get; set; }
        public double TotalDays { get; set; }
        public double TotalSalary { get; set; }
        public double TotalBorrow { get; set; }
        public double TotalPaied { get; set; }
        public double Remainder { get; set; }

    }
    public class LeaderPeriodDtos
    {
        public int BandLocationLeaderPeriodId { get; set; }
        public string PeriodName { get; set; }
        public bool PeriodState { get; set; }

    }
    public class GlobalLeaderDetail
    {
        public LeaderLocationDetail LeaderLocationDetail { get; set; }
        public List<LeaderPeriodDtos> LeaderPeriods { get; set; }
    }

    // ------------------------------------------

    public class LeaderPeriodDetail
    {
        public string LocationName { get; set; }
        public string BandName { get; set; }
        public string LeaderName { get; set; }
        public double LeaderSalary { get; set; }
        public double TotalDays { get; set; }
        public double TotalSalary { get; set; }
        public double TotalPaied { get; set; }
        public double TotalBorrow { get; set; }
        public double Remainder { get; set; }
    }
    public class UpdateLeaderPeriod
    {
        [Required]
        public double LeaderSalary { get; set; }
    }
    public class FinishLeaderPeriod
    {
        [Required]
        public double PaiedValue { get; set; }
    }





    public class AddLeaderPeriod
    {
        public IEnumerable<AddLeaderToBandRequireDto> Leaders { get; set; }
        public IEnumerable<AddPeriodRequireDto> Periods { get; set; }
    }
    public class AddLeaderToBandRequireDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class LeaderDetailsDto
    {
        public string LocationName { get; set; }

    }

}

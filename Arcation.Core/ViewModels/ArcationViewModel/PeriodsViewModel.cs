using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{

    public class AddPeriodDto
    {
        [Required]
        public string Name { get; set; }

    }
    public class UpdatePeriodDto
    {
        [Required]
        public string PeriodName { get; set; }
        [Required]
        public DateTime StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        [Required]
        public bool PeriodState { get; set; }
    }
    public class AllPeriodDto
    {
        public int PeriodId { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
    }
    // ----------------------------------------------------------
    public class PeriodDetailDto
    {
        public int PeriodId { get; set; }
        public string PeriodName { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public string LocationName { get; set; }
        public string BandName { get; set; }
        public bool PeriodState { get; set; }
        public double TotalTransictions { get; set; }
        public double TotalWesteds { get; set; }
        public double TotalDays { get; set; }
        public double CountOfEmployees { get; set; }
        public double CountOfLeaders { get; set; }
        public double TotalSalaryOfEmployees { get; set; }
        public double TotalPaied { get; set; }
        public double RemainderFromTransictions { get; set; }
        public double RemainderForEmployees { get; set; }

    }
    public class PeriodLeaders
    {
        public int BandLocationLeaderPeriodId { get; set; }
        public string LeaderID { get; set; }
        public string LeaderName { get; set; }
        public bool LeaderState { get; set; }
    }

    public class PeriodEmployees
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }

    public class GlobalSinglePeriod
    {
        public PeriodDetailDto PeriodDetail { get; set; }
        public IEnumerable<PeriodLeaders> PeriodLeaders { get; set; }
        public List<PeriodEmployees> PeriodEmployees { get; set; }
    }
    // ----------------------------------------------------------



    public class AddPeriodRequireDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
    }
    public class PeriodsLeaderApp
    {
        public int BandLocationLeaderPeriodId { get; set; }
        public int PeriodId { get; set; }
        public string PeriodName { get; set; }
        public bool PeriodState { get; set; }
    }


}

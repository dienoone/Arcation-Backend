using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class BandLocationLeaderPeriodEmployeeDto
    {
        public int BandLocationLeaderPeriodEmployeeId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeType { get; set; }
        public bool State { get; set; }
        public IEnumerable<PeriodsList> Periods { get; set; }

    }

    public class PeriodsList
    {
        public int BandLocationLeaderPeriodEmployeePeriodId { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public bool State { get; set; }
    }

    public class AddBandLocationLeaderPeriodEmployeeDto
    {
        [Required]
        public int BandLocationLeaderPeriodId { get; set; }
        [Required]
        public List<int> EmployeeIds { get; set; }
    }
    public class UpdateBandLocationLeaderPeriodEmployeeDto : AddBandLocationLeaderPeriodEmployeeDto
    {
 
        [Required]
        public List<int> oldList { get; set; }
    }


}

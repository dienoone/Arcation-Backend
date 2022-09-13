using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class AddBandLeaderDto
    {
        [Required]
        public string LeaderId { get; set; }
        [Required]
        public int BandLocationId { get; set; }
        [Required]
        public List<int> PeriodIds { get; set; }
    }
    
    public class AssignPeriodToLeaderDto
    {
        [Required]
        public int BandLocationLeaderId { get; set; }
        [Required]
        public List<int> PeriodIds { get; set; }
    }

    public class BandLocationLeaderPeriodsDto
    {
        public int BandLocationLeaderId { get; set; }
        public string LeaderName { get; set; }
        public double LeaderSalary { get; set; }
        public List<LeaderPeriods> LeaderPeriods { get; set; }
    }
    public class LeaderPeriods
    {
        public int bandLocationLeaderPeriodId { get; set; }
        public string PeriodName { get; set; }
        public bool periodState { get; set; }
        public int PeriodId { get; set; }
    }

    public class UpdateBandLocationLeaderPeriodsDto : AssignPeriodToLeaderDto
    {
        [Required]
        public List<int> OldList { get; set; }
    }

}

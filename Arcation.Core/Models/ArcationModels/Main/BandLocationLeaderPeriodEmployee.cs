using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class BandLocationLeaderPeriodEmployee : AuditModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int BandLocationLeaderPeriodId { get; set; }
        public BandLocationLeaderPeriod BandLocationLeaderPeriod { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public double EmployeeSalay { get; set; }
        public bool State { get; set; }

        // Relateions: One To Many:
        public ICollection<BandLocationLeaderPeriodEmployeePeriod> BandLocationLeaderPeriodEmployeePeriods { get; set; }
    }
}

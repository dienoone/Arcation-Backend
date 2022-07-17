using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class BandLocationLeaderPeriodEmployeePeriod : AuditModel
    {
        public int Id { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public double TotalBorrow { get; set; }
        public bool State { get; set; }
        public bool PayiedState { get; set; }
        public double PayiedValue { get; set; }
        public double EmployeeSalary { get; set; }

        // Forign Key:
        public int BandLocationLeaderPeriodEmployeeId { get; set; }
        public BandLocationLeaderPeriodEmployee BandLocationLeaderPeriodEmployee { get; set; }

        // Relations 
        public ICollection<BandLocationLeaderPeriodEmployeePeriodAttendance> BandLocationLeaderPeriodEmployeePeriodAttendances { get; set; }


    }
}

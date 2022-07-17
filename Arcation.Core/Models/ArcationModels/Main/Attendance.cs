using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Attendance : AuditModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int BandLocationLeaderPeriodId { get; set; }
        public BandLocationLeaderPeriod BandLocationLeaderPeriod { get; set; }
        public double BorrowValue { get; set; }
        public double WorkingHours { get; set; }
        public bool AttendanceState { get; set; }
        public bool ended { get; set; }

        // Relations: 
        public ICollection<BandLocationLeaderPeriodEmployeePeriodAttendance> BandLocationLeaderPeriodEmployeePeriodAttendances { get; set; }
    }
}

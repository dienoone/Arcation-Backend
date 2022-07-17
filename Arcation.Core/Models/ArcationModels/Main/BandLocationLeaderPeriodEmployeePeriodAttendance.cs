using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class BandLocationLeaderPeriodEmployeePeriodAttendance : AuditModel
    {
        public int Id { get; set; }
        public int AttendanceId { get; set; }
        public Attendance Attendance { get; set; }
        public int BandLocationLeaderPeriodEmployeePeriodId { get; set; }
        public BandLocationLeaderPeriodEmployeePeriod BandLocationLeaderPeriodEmployeePeriod { get; set; }
        public bool State { get; set; }
        public double BorrowValue { get; set; }
        public double WorkingHours { get; set; }
    }

}

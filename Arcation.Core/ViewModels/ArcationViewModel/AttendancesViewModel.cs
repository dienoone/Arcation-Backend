using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class AttendanceDto
    {
        public int AttendanceId { get; set; }
        public int BandLocationLeaderPeriodId { get; set; }
        public string LeaderName { get; set; }
        public bool AttendanceState { get; set; }
        public double BorrowValue { get; set; }
        public double WorkingHours { get; set; }
        public List<AttendanceEmployeeDto> Employees { get; set; }
    }
    public class AttendanceEmployeeDto
    {
        [Required]
        public int BandLocationLeaderPeriodEmployeePeriodAttendaceId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeType { get; set; }
        [Required]
        public bool AttendanceState { get; set; }
        [Required]
        public double BorrowValue { get; set; }
        [Required]
        public double WorkingHours { get; set; }
        [Required]
        public bool IsLeader { get; set; }

    }
    public class TakeAttendanceDto
    {
        [Required]
        public bool AttendanceState { get; set; }
        [Required]
        public double BorrowValue { get; set; }
        [Required]
        public double WorkingHours { get; set; }
        [Required]
        public List<TakeAttendanceEmployee> Employees { get; set; }
    }
    public class TakeAttendanceEmployee
    {
        [Required]
        public int BandLocationLeaderPeriodEmployeePeriodAttendaceId { get; set; }
        [Required]
        public bool AttendanceState { get; set; }
        [Required]
        public double BorrowValue { get; set; }
        [Required]
        public double WorkingHours { get; set; }
    }
    public class AllAttendances
    {
        public int AttendanceId { get; set; }
        public DateTime Date { get; set; }
        public bool ended { get; set; }
    }
    public class UpdateAttendance
    {
        [Required]
        public DateTime Date { get; set; }
    }
}

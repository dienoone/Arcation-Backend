using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class BandLocationLeaderPeriod : AuditModel
    {
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public Period Period { get; set; }
        public int BandLocationLeaderId { get; set; }
        public BandLocationLeader BandLocationLeader { get; set; }

        public DateTime StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public bool State { get; set; }
        public bool PayiedState { get; set; }
        public double TotalPaied { get; set; }
        public double LeaderSalary { get; set; }


        // Relations: one to many
        public ICollection<Transiction> Transactions { get; set; }
        public ICollection<Wested> Westeds { get; set; }
        public ICollection<BandLocationLeaderPeriodEmployee> BandLocationLeaderPeriodEmployees { get; set; }
        public ICollection<Attendance> Attendances { get; set; }

    }
}

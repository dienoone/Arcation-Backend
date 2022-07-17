using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Period : AuditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public bool State { get; set; }
        public int BandLocationId { get; set; }
        public BandLocation BandLocation { get; set; }


        // Relations:
        public ICollection<BandLocationLeaderPeriod> BandLocationLeaderPeriods { get; set; }

    }
}

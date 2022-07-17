using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class BandLocationLeader : AuditModel
    {
        public int Id { get; set; }
        public string LeaderId { get; set; }
        public Leader Leader { get; set; }
        public int BandLocationId { get; set; }
        public BandLocation BandLocation { get; set; }

        // Relations:
        public ICollection<BandLocationLeaderPeriod> BandLocationLeaderPeriods { get; set; }
    }
}

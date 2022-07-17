using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class BandLocation : AuditModel
    {
        public int Id { get; set; }

        public int BandId { get; set; }
        public Band Band { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        // Relations:
        public ICollection<Bill> Bills { get; set; }
        public ICollection<BLWested> BLWesteds { get; set; }
        public ICollection<Income> Incomes { get; set; }
        public ICollection<Period> Periods { get; set; }
        public ICollection<Extract> Extracts { get; set; }

        // Leader Relations:
        public ICollection<BandLocationLeader> BandLocationLeaders { get; set; }

    }
}

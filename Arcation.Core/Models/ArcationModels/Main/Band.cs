using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Band : AuditModel
    {
        public int Id { get; set; }
        public string BandName { get; set; }

        public List<BandLocation> BandLocations { get; set; }
        public ICollection<BandLocationLeader> BandLeaders { get; set; }
    }
}

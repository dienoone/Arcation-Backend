using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Extract : AuditModel
    {
        public int ExtractId { get; set; }
        public string ExtractName { get; set; }
        public int BandLocationId { get; set; }
        public BandLocation BandLocation { get; set; }

        // Relations: 
        public ICollection<ExtractRow> ExtractRows { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Leader : AuditModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Salary { get; set; }
        public string UserName { get; set; }
        public string Passwrod { get; set; }

        public string Photo { get; set; }
        public string IdentityPhoto { get; set; }

        // Relations: Many To Many (BandLocation && Leader)
        public ICollection<BandLocationLeader> BandLocationLeaders { get; set; }
    }
}

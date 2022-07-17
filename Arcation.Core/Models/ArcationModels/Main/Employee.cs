using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Employee : AuditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Salary { get; set; }
        public string Photo { get; set; }
        public string IdentityPhoto { get; set; }

        // forign key:
        public int TypeId { get; set; }
        public EmployeeType Type { get; set; }

        // Relations: 
        public ICollection<BandLocationLeaderPeriodEmployee> BandLocationLeaderPeriodEmployees { get; set; }

    }
}

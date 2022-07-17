using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class EmployeeType : AuditModel
    {
        public int Id { get; set; }
        public string Type { get; set; }

        // relations : {one to many}
        public ICollection<Employee> Employees { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Arcation.Core.Models.ArcationModels
{
    public class AuditModel
    {
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string BusinessId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Tool : AuditModel
    {
        public int ToolId { get; set; }
        public string ToolName { get; set; }
        public double Count { get; set; }
    }
}

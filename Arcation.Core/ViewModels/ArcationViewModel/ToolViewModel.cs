using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class AddTool
    {
        [Required]
        public string ToolName { get; set; }
        [Required]
        public double ToolCount { get; set; }
    }
    public class UpdateTool : AddTool { }

    public class ToolViewModel
    {
        public string ToolName { get; set; }
        public int ToolCount { get; set; }
    }
}

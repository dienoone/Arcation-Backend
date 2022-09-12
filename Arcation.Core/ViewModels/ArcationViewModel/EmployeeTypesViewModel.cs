using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class EmployeeTypeDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
    }
    public class AddEmployeeTypeDto : UpdateEmployeeTypeDto
    {
    }

    public class UpdateEmployeeTypeDto
    {
        [Required]
        public string Type { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class AssignEmployeesToBandLocationLeaderPeriodDto
    {
        [Required]
        public int BandLocationLeaderPeriodId { get; set; }
        [Required]
        public List<int> EmployeeIds { get; set; }
    }

    public class FinishSubPeriodDto
    {
       

    }

    public class UpdateSubPeriod
    {
        [Required]
        public double EmployeeSalary { get; set; }
    }
    public class UpdateMainPeriodForEmployee
    {
        [Required]
        public bool EmployeeState { get; set; }

    }
}

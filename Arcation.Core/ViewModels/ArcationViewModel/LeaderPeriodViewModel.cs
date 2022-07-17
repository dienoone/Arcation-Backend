using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{

    public class BandLocationLeaderPeriodDetailDto
    {
        public PeriodDetail PeriodDetail { get; set; }
        public LeaderDetail LeaderDetail { get; set; }
        public EmployeesDetail EmployeesDetail { get; set; }

    }
    public class PeriodDetail
    {
        public double TotalTransictions { get; set; }
        public double TotalWesteds { get; set; }
        public double TotalBorrows { get; set; }
        public double Remainder { get; set; }
        public double TotalDays { get; set; }

    }
    public class EmployeesDetail
    {
        public double TotalSalaryOfEmployess { get; set; }
        public double TotalPaied { get; set; }
        public double TotalRemainder { get; set; }
        public List<EmployeeTypesDetail> employeeTypesDetails { get; set; }


    }
    public class EmployeeTypesDetail
    {
        public string TypeName { get; set; }
        public double TotalEmployess { get; set; }
    }

    public class LeaderDetail
    {
        public string LeaderName { get; set; }
        public double LeaderSalary { get; set; }
        public double TotalDays { get; set; }
        public double TotalSalary { get; set; }
        public double TotalBorrow { get; set; }
        public bool IsEnded { get; set; }
        public double TotalPaied { get; set; }
        public double TotalRemainder { get; set; }

    }

}

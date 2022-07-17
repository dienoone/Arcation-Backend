using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class BLReport
    {
        public BLCompanyReport CompanyReport { get; set; }
        public BLLeadersReport LLeadersReport { get; set; }
        public EmployeesDetail EmployeesReport { get; set; }

    }

    public class BLCompanyReport
    {
        public double TotalBills { get; set; }
        public double TotalSalaryOfEmployees { get; set; }
        public double TotalIncomes { get; set; }
        public double TotalWested { get; set; }
        public double TotalExtracts { get; set; }
        public double Remainder { get; set; }
    }

    public class BLLeadersReport
    {
        public double TotalTransictions { get; set; }
        public double TotalWesteds { get; set; }
        public double TotalSalaryOfLeaders { get; set; }
        public double TotalPaied { get; set; }
        public double Remainder { get; set; }
    }

    // Global Report:
    public class GlobalAccount
    {
        public GlobalAccountWork GlobalAccountWork { get; set; }
        public GlobalAccountMoney GlobalAccountMoney { get; set; }
    }

    public class GlobalAccountWork
    {
        public string LocationName { get; set; }
        public string CompanyName { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public int NumberOfCompanies { get; set; }
        public int NumberOfLocations { get; set; }
        public int NumberOfBands { get; set; }
        public int NumberOfLeaders { get; set; }
        public int NumberOfEmployees { get; set; }
        public List<EmployeeTypesDetail> EmployeeTypesDetails { get; set; }
    }

    public class GlobalAccountMoney
    {
        public double TotalExtracts { get; set; }
        public double TotalIncomes { get; set; }
        public double TotalWesteds { get; set; }
        public double TotalBills { get; set; }
        public double TotalTransictions { get; set; }
        public double TotalSalaryOFEmployees { get; set; }
        public double TotalSalaryOFLeaders { get; set; }
        public double GlobalSalary { get; set; }
        public double TotalPayied { get; set; }
        public double RemainderSalary { get; set; }
        public double RemainderIncome { get; set; }
        public double Profit { get; set; }

    }


    public class GlobalReport
    {
        public GlobalAccount globalAccount { get; set; }
        public List<CompanyReprots> Companies { get; set; }
        public IEnumerable<CompanyLocationBandReports> Bands { get; set; }
        public IEnumerable<CompanyLocationsReports> Locations { get; set; }

    }

    public class CompanyReprots
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
    public class CompanyLocationsReports
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }
    public class CompanyLocationBandReports
    {
        public int BandId { get; set; }
        public string BandName { get; set; }
    }

}

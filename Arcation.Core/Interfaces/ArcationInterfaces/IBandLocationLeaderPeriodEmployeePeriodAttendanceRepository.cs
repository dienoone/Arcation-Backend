using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IBandLocationLeaderPeriodEmployeePeriodAttendanceRepository : IBaseRepository<BandLocationLeaderPeriodEmployeePeriodAttendance>
    {
        double GetTotalSalaryOfEmployeeLeaderPeriodReoprt(int? bandLocationLeaderPeriodId, string busniessId);
        double GetTotalBorrowOfEmployeeLeaderPeriodReoprt(int? bandLocationLeaderPeriodId, string busniessId);
        double GetTotalBorrowOfEmployeePeriodReoprt(int? periodId, string busniessId);
        double GetTotalPaiedOfEmployeePeriodReoprt(int? periodId, string busniessId);
        double GetTotalSalaryOfEmployeePeriodReoprt(int? periodId, string busniessId);
        double GetBandLocationInnerReport(int? bandLocationId, string businessId);
        double GetBandLocationInnerReportBorrow(int? bandLocationId, string businessId);

        double GetCompanyGlobalReportSalary(int? companyId, string busiessId);
        double GetLocationGlobalReportSalary(int? locationId, string busiessId);
        double GetBandGlobalReportSalary(int? bandId, string busiessId);

        double GetCompanyGlobalReportPaied(int? companyId, string busiessId);
        double GetLocationGlobalReportPaied(int? locationId, string busiessId);
        double GetBandGlobalReportPaied(int? bandId, string busiessId);

    }
}

using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IAttendanceRepository : IBaseRepository<Attendance>
    {
        Task<Attendance> GetInializeAttendance(int attendanceId);
        Task<Attendance> TakeAttendance(int? attendanceId);
        Task<Attendance> GetAttendance(int? attendanceId, int? bandLocationLeaderPeriodId);
        Task<Attendance> GetSearchAttendance(int? attendanceId, string name, string businessID);
        double GetLeaderBorrow(int? bandLocationLeaderPeriodId, string businessId);
        double GetLeaderDays(int? bandLocationLeaderPeriodId, string businessId);
        double GetTotalBorrowOfEmployeePeriodReoprt(int? periodId, string busniessId);
        double GetTotalPaiedOfEmployeePeriodReoprt(int? periodId, string busniessId);
        double GetTotalSalaryOfEmployeePeriodReoprt(int? periodId, string busniessId);
        double GetPeriodDaysReport(int? periodId, string businessId);
        double GetBandLocationInnerReport(int? bandLocationId, string businessId);
        double GetBandLocationInnerReportPaied(int? bandLocationId, string businessId);

        double GetCompanyGlobalReportSalary(int? companyId, string busiessId);
        double GetLocationGlobalReportSalary(int? locationId, string busiessId);
        double GetBandGlobalReportSalary(int? bandId, string busiessId);

        double GetCompanyGlobalReportPaied(int? companyId, string busiessId);
        double GetLocationGlobalReportPaied(int? locationId, string busiessId);
        double GetBandGlobalReportPaied(int? bandId, string busiessId);

    }
}

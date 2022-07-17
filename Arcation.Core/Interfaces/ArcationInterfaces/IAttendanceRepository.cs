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
        double GetTotalCompanyBorrow(int? companyId);
        double GetTotalBandBorrow(int? bandId);
        double GetTotalLocationBorrow(int? locationId);
        double GetTotalCompanyHours(int? companyId);
        double GetTotalBandHours(int? bandId);
        double GetTotalLocationHours(int? locationId);

    }
}

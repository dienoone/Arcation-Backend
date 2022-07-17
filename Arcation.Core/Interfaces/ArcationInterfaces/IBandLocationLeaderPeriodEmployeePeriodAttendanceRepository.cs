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
        Task<double> GetSumOfBorrowValue(IEnumerable<int> Ids);
        double GetTotalCompanyBorrow(int? companyId);
        double GetTotalBandBorrow(int? bandId);
        double GetTotalLocationBorrow(int? locationId);
        double GetTotalCompanyHours(int? companyId);
        double GetTotalBandHours(int? bandId);
        double GetTotalLocationHours(int? locationId);
    }
}

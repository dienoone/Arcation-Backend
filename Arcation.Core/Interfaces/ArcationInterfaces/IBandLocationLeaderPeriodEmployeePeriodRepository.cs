using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IBandLocationLeaderPeriodEmployeePeriodRepository : IBaseRepository<BandLocationLeaderPeriodEmployeePeriod>
    {
        Task<BandLocationLeaderPeriodEmployeePeriod> GetSubPeriodIncludeAttendace(int? bandLocationLeaderPeriodEmployeePeriodId, string BusinessId);
        double GetTotalCompanyPaied(int? companyId);
        double GetTotalBandPaied(int? bandId);
        double GetTotalLocationPaied(int? locationId);
        double GetTotalCompanySalary(int? companyId);
        double GetTotalBandSalary(int? bandId);
        double GetTotalLocationSalary(int? locationId);

        double GetBandLocationInnerReport(int? bandLocationId, string businessId);
    }
}

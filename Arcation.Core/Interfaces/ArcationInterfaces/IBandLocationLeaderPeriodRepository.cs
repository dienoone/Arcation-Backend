using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IBandLocationLeaderPeriodRepository : IBaseRepository<BandLocationLeaderPeriod>
    {
        Task<BandLocationLeaderPeriod> GetLeaderPeriodDetail(int? bandLocationLeaderPeriodId, string businessId);
        Task<BandLocationLeaderPeriod> GetLeaderPeriodFinish(int? bandLocationLeaderPeriodId, string businessId);
        Task<IEnumerable<BandLocationLeaderPeriod>> GetPeriods(int? bandLocationLeaderId, string LeaderId, string businessID);
        double GetTotalCompanyPaied(int? companyId);
        double GetTotalBandPaied(int? bandId);
        double GetTotalLocationPaied(int? locationId);
        double GetTotalCompanySalary(int? companyId);
        double GetTotalBandSalary(int? bandId);
        double GetTotalLocationSalary(int? locationId);
    }
}

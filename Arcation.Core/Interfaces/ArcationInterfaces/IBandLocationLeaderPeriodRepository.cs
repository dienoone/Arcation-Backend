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
        Task<List<int>> GetPeriodIds(int? bandLocationLeaderId, string businessId);
        Task<BandLocationLeaderPeriod> GetLeaderPeriodDetail(int? bandLocationLeaderPeriodId, string businessId);
        Task<BandLocationLeaderPeriod> GetLeaderPeriodFinish(int? bandLocationLeaderPeriodId, string businessId);
        Task<IEnumerable<BandLocationLeaderPeriod>> GetPeriods(int? bandLocationLeaderId, string LeaderId, string businessID);
        int GetPeriodConutOfLeadersReport(int? periodId, string busniessId);
        Task<IEnumerable<BandLocationLeaderPeriod>> GetPeriodsAsync(int? periodId, string businessId);

    }
}

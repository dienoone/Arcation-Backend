using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IBandLocationLeaderRepository : IBaseRepository<BandLocationLeader>
    {
        Task<IEnumerable<BandLocationLeader>> GetLeadersWithPeriods(int? bandLocationId, string businessId);
        Task<IEnumerable<BandLocationLeader>> SearchLeadersWithPeriods(int? bandLocationId, string name, string businessId);
        Task<BandLocationLeader> GetLeaderWithPeriod(int? bandLocationId, int? bandLocationLeaderId, string businessId);
        Task<BandLocationLeader> GetLeaderWithPeriod(int? bandLocationLeaderId, string businessId);
        Task<IEnumerable<BandLocationLeader>> GetForLeader(string leaderId, string businessId);
        Task<BandLocationLeader> GetForLeaderDetail(int? bandLocationLeaderId, string BuisnessId);


        int NumberOfLeadersCompany(int? companyId, string businessId);
        int NumberOfLeadersLocation(int? locationId, string businessId);
        int NumberOfLeadersBand(int? bandId, string businessId);

        List<int> GetLocationIdsForLeader(string leaderId, string businessId);
        BandLocationLeader GetForLeader(string leaderId, int locationId);
        List<int> GetBandIdsForLeaderLocation(int locationId, string leaderId);
    }
}

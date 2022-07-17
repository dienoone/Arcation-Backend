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
        Task<BandLocationLeader> GetLeaderWithPeriod(int? bandLocationId, int? bandLocationLeaderId, string businessId);
        Task<BandLocationLeader> GetLeaderWithPeriod(int? bandLocationLeaderId, string businessId);
        Task<IEnumerable<BandLocationLeader>> GetForLeader(string leaderId, string businessId);
        Task<BandLocationLeader> GetForLeaderDetail(int? bandLocationLeaderId, string BuisnessId);
    }
}

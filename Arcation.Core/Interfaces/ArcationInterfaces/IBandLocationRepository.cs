using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IBandLocationRepository : IBaseRepository<BandLocation>
    {
        Task<List<int>> GetBandsId(int locationId);
        Task<BandLocation> Reports(int? bandLocationId, string BusinessId);
        Task<BandLocation> GetBandLocationReport(int? bandLocationId, string businessId);
    }
}

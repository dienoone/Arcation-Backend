using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IExtractRepository : IBaseRepository<Extract>
    {
        Task<Extract> GetExtractAsync(int? bandLocationId, string businessId);
        Task<IEnumerable<Extract>> GetExtractsAsync(int? bandLocationId, string businessId);

    }
}

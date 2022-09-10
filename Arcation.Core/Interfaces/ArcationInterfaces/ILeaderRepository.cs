using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface ILeaderRepository : IBaseRepository<Leader>
    {
        Task<IEnumerable<Leader>> GetForAdd(string businessId);
        Task<IEnumerable<Leader>> SearchForAdd(string name, string businessId);
        Task<Leader> GetLeaderDetail(string LeaderId, string BusinessId);
    }
}

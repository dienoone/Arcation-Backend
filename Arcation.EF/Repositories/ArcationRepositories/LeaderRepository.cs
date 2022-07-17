using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models.ArcationModels.Main;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF.Repositories.ArcationRepositories
{
    public class LeaderRepository : BaseRepository<Leader>, ILeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public LeaderRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Leader>> GetForAdd(string businessId)
        {
            return await _context.Leaders
                .Include(e => e.BandLocationLeaders.Where(e => !e.IsDeleted))
                .Where(e => e.BusinessId == businessId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<Leader> GetLeaderDetail(string LeaderId, string BusinessId)
        {
            return await _context.Leaders
                .Include(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Attendances)
                .Include(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Period)
                .Include(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocation).ThenInclude(e => e.Band)
                .Include(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocation).ThenInclude(e => e.Location)
                .FirstOrDefaultAsync(e => e.Id == LeaderId && e.BusinessId == BusinessId);
        }
    }
}

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
    public class BandLocationLeaderPeriodRepository : BaseRepository<BandLocationLeaderPeriod>, IBandLocationLeaderPeriodRepository
    {
        private readonly ApplicationDbContext _context;

        public BandLocationLeaderPeriodRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<int>> GetPeriodIds(int? bandLocationLeaderId, string businessId)
        {
            return await _context.BandLocationLeaderPeriods
                .Where(e => e.BandLocationLeaderId == bandLocationLeaderId && !e.IsDeleted && e.BusinessId == businessId)
                .Select(e => e.PeriodId)
                .ToListAsync();

        }

        public async Task<BandLocationLeaderPeriod> GetLeaderPeriodDetail(int? bandLocationLeaderPeriodId, string businessId)
        {
            return await _context.BandLocationLeaderPeriods
                .Include(e => e.BandLocationLeader.Leader)
                .Where(e => !e.IsDeleted && e.BusinessId == businessId && e.Id == bandLocationLeaderPeriodId).FirstOrDefaultAsync();
        }

        public async Task<BandLocationLeaderPeriod> GetLeaderPeriodFinish(int? bandLocationLeaderPeriodId, string businessId)
        {
            return await _context.BandLocationLeaderPeriods
                .Include(e => e.BandLocationLeader).ThenInclude(e => e.Leader)
                .Include(e => e.BandLocationLeader).ThenInclude(e => e.BandLocation).ThenInclude(e => e.Location)
                .Include(e => e.BandLocationLeader).ThenInclude(e => e.BandLocation).ThenInclude(e => e.Band)
                .Include(e => e.Attendances)
                .FirstOrDefaultAsync(e => e.Id == bandLocationLeaderPeriodId && e.BusinessId == businessId);
        }

        public async Task<IEnumerable<BandLocationLeaderPeriod>> GetPeriods(int? bandLocationLeaderId, string LeaderId, string businessID)
        {
            return await _context.BandLocationLeaderPeriods
                .Include(e => e.Period)
                .Where(e => e.BandLocationLeader.LeaderId == LeaderId && e.BandLocationLeaderId == bandLocationLeaderId && e.BusinessId == businessID)
                .ToListAsync();
        }

        public int GetPeriodConutOfLeadersReport(int? periodId, string busniessId)
        {
            return _context.BandLocationLeaderPeriods
                .Where(e => e.PeriodId == periodId && !e.IsDeleted && e.BusinessId == busniessId)
                .Count();
        }

        public async Task<IEnumerable<BandLocationLeaderPeriod>> GetPeriodsAsync(int? periodId, string businessId)
        {
            return await _context.BandLocationLeaderPeriods
                .Include(e => e.BandLocationLeader.Leader)
                .Where(e => e.PeriodId == periodId && e.BusinessId == businessId && !e.IsDeleted && !e.BandLocationLeader.Leader.IsDeleted)
                .ToListAsync();
        }

       
    }
}

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
    public class BandLocationLeaderRepository : BaseRepository<BandLocationLeader>, IBandLocationLeaderRepository
    {
        private readonly ApplicationDbContext _context;

        public BandLocationLeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BandLocationLeader>> GetLeadersWithPeriods(int? bandLocationId, string businessId)
        {
            return await _context.BandLocationLeaders.Include(bll => bll.Leader)
                .Include(bll => bll.BandLocationLeaderPeriods.Where(e => !e.IsDeleted)).ThenInclude(e => e.Period).Where(bll => bll.BandLocationId == bandLocationId && bll.BusinessId == businessId &&
                !bll.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<BandLocationLeader>> SearchLeadersWithPeriods(int? bandLocationId, string name ,string businessId)
        {
            return await _context.BandLocationLeaders.Include(bll => bll.Leader)
                .Include(bll => bll.BandLocationLeaderPeriods.Where(e => !e.IsDeleted)).ThenInclude(e => e.Period).Where(bll => bll.BandLocationId == bandLocationId && bll.BusinessId == businessId &&
                !bll.IsDeleted && bll.Leader.Name.Contains(name)).ToListAsync();
        }

        public async Task<BandLocationLeader> GetLeaderWithPeriod(int? bandLocationId,int? bandLocationLeaderId, string businessId)
        {
            return await _context.BandLocationLeaders.Include(bll => bll.Leader)
                .Include(bll => bll.BandLocationLeaderPeriods.Where(e => !e.IsDeleted)).ThenInclude(e => e.Period).Where(bll => bll.BandLocationId == bandLocationId && bll.BusinessId == businessId &&
                !bll.IsDeleted && bll.Id == bandLocationLeaderId).FirstOrDefaultAsync();
        }

        public async Task<BandLocationLeader> GetLeaderWithPeriod(int? bandLocationLeaderId, string businessId)
        {
            return await _context.BandLocationLeaders.Include(bll => bll.Leader)
                .Include(bll => bll.BandLocationLeaderPeriods.Where(e => !e.IsDeleted))
                .Where(bll => bll.BusinessId == businessId && !bll.IsDeleted && bll.Id == bandLocationLeaderId)
                .FirstOrDefaultAsync();
        }

        // Leaders Controller:
        public async Task<IEnumerable<BandLocationLeader>> GetForLeader(string leaderId, string businessId)
        {
            return await _context.BandLocationLeaders
                .Include(e => e.BandLocation.Band)
                .Include(e => e.BandLocation.Location)
                .Include(e => e.BandLocationLeaderPeriods.Where(e => !e.IsDeleted))
                .Where(e => e.LeaderId == leaderId && e.BusinessId == businessId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<BandLocationLeader> GetForLeaderDetail(int? bandLocationLeaderId, string BuisnessId)
        {
            return await _context.BandLocationLeaders
                .Include(e => e.Leader)
                .Include(e => e.BandLocation).ThenInclude(e => e.Band)
                .Include(e => e.BandLocation).ThenInclude(e => e.Location)
                .Include(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Attendances)
                .Include(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Period)
                .Where(e => e.Id == bandLocationLeaderId && e.BusinessId == BuisnessId)
                .FirstOrDefaultAsync();
        }

        public int NumberOfLeadersCompany(int? companyId, string businessId)
        {
            return _context.BandLocationLeaders
                .Where(e => e.BandLocation.Location.CompanyId == companyId && !e.IsDeleted && e.BusinessId == businessId)
                .Count();
        }

        public int NumberOfLeadersLocation(int? locationId, string businessId)
        {
            return _context.BandLocationLeaders
                .Where(e => e.BandLocation.LocationId == locationId && !e.IsDeleted && e.BusinessId == businessId)
                .Count();
        }

        public int NumberOfLeadersBand(int? bandId, string businessId)
        {
            return _context.BandLocationLeaders
                .Where(e => e.BandLocation.Id == bandId && !e.IsDeleted && e.BusinessId == businessId)
                .Count();
        }


    }
}

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
    public class BandLocationRepository : BaseRepository<BandLocation>, IBandLocationRepository
    {
        private readonly ApplicationDbContext _context;

        public BandLocationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<int>> GetBandsId(int locationId)
        {
            return await _context.BandLocations.Where(e => e.LocationId == locationId).Select(e => e.BandId).ToListAsync();
        }

        public async Task<BandLocation> Reports(int? bandLocationId, string BusinessId)
        {
            return await _context.BandLocations
                .Include(e => e.Bills)
                .Include(e => e.BLWesteds)
                .Include(e => e.Incomes)
                .Include(e => e.Periods)
                .Include(e => e.Extracts).ThenInclude(e => e.ExtractRows)
                .Include(c => c.BandLocationLeaders).ThenInclude(a => a.BandLocationLeaderPeriods).ThenInclude(e => e.Transactions)
                .Include(c => c.BandLocationLeaders).ThenInclude(a => a.BandLocationLeaderPeriods).ThenInclude(e => e.Westeds)
                .Include(c => c.BandLocationLeaders).ThenInclude(a => a.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployees).ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances)
                .Include(c => c.BandLocationLeaders).ThenInclude(a => a.BandLocationLeaderPeriods).ThenInclude(e => e.Attendances)
                .FirstOrDefaultAsync(e => e.Id == bandLocationId && e.BusinessId == BusinessId);
        }

        public async Task<BandLocation> GetBandLocationReport(int? bandLocationId, string businessId)
        {
            return await _context.BandLocations
                .Include(e => e.Location).ThenInclude(e => e.Company)
                .Include(e => e.Band)
                .Include(e => e.Bills)
                .Include(e => e.BLWesteds)
                .Include(e => e.Incomes)
                .Include(e => e.Periods)
                .Include(e => e.Extracts).ThenInclude(e => e.ExtractRows)
                .Include(e => e.BandLocationLeaders).ThenInclude(e => e.Leader)
                .Include(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Transactions)
                .Include(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Westeds)
                .Include(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Attendances)
                .Include(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployees).ThenInclude(e => e.Employee).ThenInclude(e => e.Type)
                .Include(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployees).ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances)
                .FirstOrDefaultAsync(e => e.Id == bandLocationId && e.BusinessId == businessId);
        }
    }
}

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
    public class PeriodRepository  : BaseRepository<Period>, IPeriodRepository
    {
        private readonly ApplicationDbContext _context;
        public PeriodRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        public async Task<Period> GetPeriodIncludeBandLocation(int? periodId, string businessId)
        {
            return await _context.Periods
                .Include(e => e.BandLocation.Band)
                .Include(e => e.BandLocation.Location)
                .FirstOrDefaultAsync(e => e.Id == periodId && !e.IsDeleted && e.BusinessId == businessId);
        }


        public async Task<Period> GetPeriodDetail(int? periodId, string BusinessId)
        {
            return await _context.Periods
                .Include(e => e.BandLocation).ThenInclude(e => e.Band)
                .Include(e => e.BandLocation).ThenInclude(e => e.Location)
                .Include(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeader).ThenInclude(e => e.Leader)
                .Include(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeader).ThenInclude(e => e.BandLocation).ThenInclude(e => e.Location)
                .Include(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeader).ThenInclude(e => e.BandLocation).ThenInclude(e => e.Band)
                .Include(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Transactions)
                .Include(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Westeds)
                .Include(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Attendances)
                .Include(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployees).ThenInclude(e => e.Employee)
                .Include(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployees).ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances)
                .FirstOrDefaultAsync(e => e.Id == periodId && e.BusinessId == BusinessId);

        }
    }
}

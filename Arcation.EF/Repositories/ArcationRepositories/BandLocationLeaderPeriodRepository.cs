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

        public async Task<BandLocationLeaderPeriod> GetLeaderPeriodDetail(int? bandLocationLeaderPeriodId, string businessId)
        {
            return await _context.BandLocationLeaderPeriods
                .Include(e => e.BandLocationLeader.Leader)
                .Include(e => e.Transactions.Where(e => !e.IsDeleted))
                .Include(e => e.Westeds.Where(e => !e.IsDeleted))
                .Include(e => e.Attendances.Where(e => !e.IsDeleted))
                .Include(c => c.BandLocationLeaderPeriodEmployees.Where(c => !c.IsDeleted)).ThenInclude(c => c.Employee).ThenInclude(z => z.Type)
                .Include(c => c.BandLocationLeaderPeriodEmployees.Where(c => !c.IsDeleted))
                .ThenInclude(c => c.BandLocationLeaderPeriodEmployeePeriods).ThenInclude(a => a.BandLocationLeaderPeriodEmployeePeriodAttendances)
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

        public double GetTotalCompanyPaied(int? companyId)
        {
            return _context.BandLocationLeaderPeriods
                .Where(e => e.BandLocationLeader.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.TotalPaied);
        }
        public double GetTotalBandPaied(int? bandId)
        {
            return _context.BandLocationLeaderPeriods
                .Where(e => e.BandLocationLeader.BandLocation.Id == bandId)
                .Sum(e => e.TotalPaied);
        }
        public double GetTotalLocationPaied(int? locationId)
        {
            return _context.BandLocationLeaderPeriods
                .Where(e => e.BandLocationLeader.BandLocation.LocationId == locationId)
                .Sum(e => e.TotalPaied);
        }
        public double GetTotalCompanySalary(int? companyId)
        {
            return _context.BandLocationLeaderPeriods
                .Where(e => e.BandLocationLeader.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.LeaderSalary);
        }
        public double GetTotalBandSalary(int? bandId)
        {
            return _context.BandLocationLeaderPeriods
                .Where(e => e.BandLocationLeader.BandLocation.Id == bandId)
                .Sum(e => e.LeaderSalary);
        }
        public double GetTotalLocationSalary(int? locationId)
        {
            return _context.BandLocationLeaderPeriods
                .Where(e => e.BandLocationLeader.BandLocation.LocationId == locationId)
                .Sum(e => e.LeaderSalary);
        }

    }
}

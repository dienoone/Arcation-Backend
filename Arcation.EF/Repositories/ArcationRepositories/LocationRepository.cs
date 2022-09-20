using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF.Repositories.ArcationRepositories
{
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        private ApplicationDbContext _context;

        public LocationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Location> LocationAsync(int locationId)
        {
            return await _context.Locations
                .Include(e => e.BandLocations.Where(e => !e.IsDeleted))
                .ThenInclude(e => e.Band)
                .FirstOrDefaultAsync(e => e.Id == locationId && !e.IsDeleted);
        }

        public async Task<List<Location>> GetAllLocationsAsync(string userID, int? companyId)
        {
            return await _context.Locations.Include(src => src.BandLocations.Where(e => e.IsDeleted == false))
                .ThenInclude(b => b.Band).Where(l => l.BusinessId == userID && l.IsDeleted == false && l.CompanyId == companyId).ToListAsync();
        }

        public async Task<Location> GetLocation(string BusinessId, int? locationId)
        {
            return await _context.Locations
                .Include(e => e.BandLocations.Where(e => !e.IsDeleted))
                .ThenInclude(e => e.Band)
                .FirstOrDefaultAsync(e => e.Id == locationId && e.BusinessId == BusinessId && !e.IsDeleted);
        }

        public async Task<List<Location>> SearchLocationAsync(string userId, int? companyId, string name)
        {
            return await _context.Locations.Include(src => src.BandLocations.Where(e => e.IsDeleted == false))
                .ThenInclude(b => b.Band).Where(l => l.BusinessId == userId && l.IsDeleted == false && l.CompanyId == companyId && l.LocationName.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<Location>> LocationRelatedToEmployee(int? EmployeeId, string businessId)
        {
            return await _context.Locations.Where(e => e.BusinessId == businessId && e.BandLocations
            .Any(e => e.BandLocationLeaders.Any(e => e.BandLocationLeaderPeriods.Any(e => e.BandLocationLeaderPeriodEmployees.Any(e => e.EmployeeId == EmployeeId))))).ToListAsync();
        }

        public async Task<IEnumerable<Location>> LocaionsWithBandsRelatedToLeader(string leaderId, string BusinessId)
        {
            return await _context.Locations
                .Include(e => e.BandLocations.Where(e => e.BandLocationLeaders.Any(e => e.LeaderId == leaderId))).ThenInclude(e => e.Band)
                .Where(e => e.BusinessId == BusinessId && e.BandLocations.Any(e => e.BandLocationLeaders.Any(e => e.LeaderId == leaderId && e.BusinessId == BusinessId))).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SearchLocaionsWithBandsRelatedToLeader(string leaderId, string BusinessId, string name)
        {
            return await _context.Locations
                .Include(e => e.BandLocations.Where(e => e.BandLocationLeaders.Any(e => e.LeaderId == leaderId) && !e.IsDeleted)).ThenInclude(e => e.Band)
                .Where(e => e.BusinessId == BusinessId && !e.IsDeleted && e.LocationName.Contains(name) && e.BandLocations.Any(e => e.BandLocationLeaders.Any(e => e.LeaderId == leaderId && e.BusinessId == BusinessId))).ToListAsync();
        }

        public async Task<Location> GetLocationReport(int? locationId, string businessId)
        {
            return await _context.Locations
                .Include(e => e.Company)
                .Include(e => e.BandLocations).ThenInclude(e => e.Band)
                .Include(e => e.BandLocations).ThenInclude(e => e.Extracts)
                .Include(e => e.BandLocations).ThenInclude(e => e.BLWesteds)
                .Include(e => e.BandLocations).ThenInclude(e => e.Incomes)
                .Include(e => e.BandLocations).ThenInclude(e => e.BLWesteds)
                .Include(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.Leader)
                .Include(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Transactions)
                .Include(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Westeds)
                .Include(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Attendances)
                .Include(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployees).ThenInclude(e => e.Employee).ThenInclude(e => e.Type)
                .Include(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployees).ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances)
                .FirstOrDefaultAsync(e => e.Id == locationId && e.BusinessId == businessId);

        }

        public int NumberOfLocationCompany(int? companyId, string busniessId)
        {
            return _context.Locations
                .Where(e => e.CompanyId == companyId && e.BusinessId == busniessId && !e.IsDeleted)
                .Count();
        }

    }
}

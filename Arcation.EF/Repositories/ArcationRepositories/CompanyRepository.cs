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
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GlobalReport(string BusinessId)
        {
            return await _context.Companies
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.Bills)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.BLWesteds)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.Incomes)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.Extracts)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Transactions)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Westeds)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Attendances)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployees)
                .ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances)
                .Where(e => e.BusinessId == BusinessId).ToListAsync();
        }

        public async Task<Company> SingleGlobalReport(string BusinessId, int? Id)
        {
            return await _context.Companies
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.Bills)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.BLWesteds)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.Incomes)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.Extracts)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Transactions)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Westeds)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.Attendances)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployees).ThenInclude(e => e.Employee).ThenInclude(e => e.Type)
                .Include(e => e.Locations).ThenInclude(e => e.BandLocations).ThenInclude(e => e.BandLocationLeaders).ThenInclude(e => e.BandLocationLeaderPeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployees)
                .ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriods).ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances)
                .FirstOrDefaultAsync(e => e.BusinessId == BusinessId && e.Id == Id);
        }


    }
}

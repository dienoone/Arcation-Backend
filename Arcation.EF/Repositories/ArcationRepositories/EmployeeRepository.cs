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
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllIncludeTypes(string businessId)
        {
            return await _context.Employees
                .Include(e => e.Type)
                .Where(e => e.BusinessId == businessId && !e.IsDeleted && !e.Type.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetSearch(string name ,string businessId)
        {
            return await _context.Employees
                .Include(e => e.Type)
                .Where(e => e.BusinessId == businessId && !e.IsDeleted && !e.Type.IsDeleted && e.Name.Contains(name)).ToListAsync();
        }

        public async Task<Employee> GetEmployeeIncludeTypes(int? Id,string businessId)
        {
            return await _context.Employees
                .Include(e => e.Type)
                .Where(e => e.BusinessId == businessId && !e.IsDeleted && !e.Type.IsDeleted && e.Id == Id).FirstOrDefaultAsync();
        }
        
        public async Task<Employee> GetEmployeePeriods(int? Id, string businessId)
        {
            return await _context.Employees
                .Include(e => e.Type)
                .Include(e => e.BandLocationLeaderPeriodEmployees)
                .ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriods)
                .ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances)
                .Include(e => e.BandLocationLeaderPeriodEmployees).ThenInclude(e => e.BandLocationLeaderPeriod).ThenInclude(e => e.BandLocationLeader)
                .ThenInclude(e => e.Leader)
                .Include(e => e.BandLocationLeaderPeriodEmployees).ThenInclude(e => e.BandLocationLeaderPeriod).ThenInclude(e => e.Period).ThenInclude(e => e.BandLocation).ThenInclude(e => e.Band)
                .Include(e => e.BandLocationLeaderPeriodEmployees).ThenInclude(e => e.BandLocationLeaderPeriod.Period.BandLocation.Location)
                .Where(e => e.Id == Id && e.BusinessId == businessId && !e.IsDeleted)
                .FirstOrDefaultAsync();
        }

    }
}

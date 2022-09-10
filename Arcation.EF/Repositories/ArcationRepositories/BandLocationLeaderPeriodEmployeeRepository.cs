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
    public class BandLocationLeaderPeriodEmployeeRepository : BaseRepository<BandLocationLeaderPeriodEmployee>, IBandLocationLeaderPeriodEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public BandLocationLeaderPeriodEmployeeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BandLocationLeaderPeriodEmployee>> GetPeriodEmployeeDataAsync(int? bandLocationLeaderPeriodId, string bussinessId)
        {

            return await _context.BandLocationLeaderPeriodEmployees.Include(bllpe => bllpe.Employee).ThenInclude(e => e.Type)
                .Include(bllpe => bllpe.BandLocationLeaderPeriodEmployeePeriods.Where(e => !e.IsDeleted)).Where(bllpe => !bllpe.IsDeleted
                && bllpe.BusinessId == bussinessId && bllpe.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && !bllpe.Employee.IsDeleted && !bllpe.Employee.Type.IsDeleted).ToListAsync();
        }

        public async Task<List<BandLocationLeaderPeriodEmployee>> SearchPeriodEmployeeDataAsync(int? bandLocationLeaderPeriodId, string name,string bussinessId)
        {

            return await _context.BandLocationLeaderPeriodEmployees.Include(bllpe => bllpe.Employee).ThenInclude(e => e.Type)
                .Include(bllpe => bllpe.BandLocationLeaderPeriodEmployeePeriods.Where(e => !e.IsDeleted)).Where(bllpe => !bllpe.IsDeleted
                && bllpe.BusinessId == bussinessId && bllpe.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && !bllpe.Employee.IsDeleted && !bllpe.Employee.Type.IsDeleted && bllpe.Employee.Name.Contains(name)).ToListAsync();
        }

        public async Task<BandLocationLeaderPeriodEmployee> GetSinglePeriodEmployeeDataAsync(int? bandLocationLeaderPeriodId, int? Id, string bussinessId)
        {
            return await _context.BandLocationLeaderPeriodEmployees.Include(bllpe => bllpe.Employee).ThenInclude(e => e.Type)
                .Include(bllpe => bllpe.BandLocationLeaderPeriodEmployeePeriods).Where(bllpe => !bllpe.IsDeleted
                && bllpe.BusinessId == bussinessId && bllpe.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && bllpe.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<BandLocationLeaderPeriodEmployee>> GetForInitializeAttendance(int? bandLocationLeaderPeriodId, string bussinessId)
        {
            return await _context.BandLocationLeaderPeriodEmployees
                .Include(e => e.Employee)
                .Include(e => e.BandLocationLeaderPeriodEmployeePeriods.Where(e => !e.IsDeleted))
                .Where(e => e.State && !e.IsDeleted && e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId && e.BusinessId == bussinessId && !e.Employee.IsDeleted).ToListAsync();
        }
        public async Task<BandLocationLeaderPeriodEmployee> GetPeriodDetailsAsync (int? locationID, int? employeeID, string busniessId)
        {
            return await _context.BandLocationLeaderPeriodEmployees
                .Include(e => e.Employee)
                .Include(e => e.BandLocationLeaderPeriodEmployeePeriods.Where(e => !e.IsDeleted))
                .ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Where(e => !e.IsDeleted))
                .Include(e => e.BandLocationLeaderPeriod.BandLocationLeader.Leader)
                .Include(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location)
                .Include(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Band)
                .Where(e => e.EmployeeId == employeeID && e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationID && e.BusinessId == busniessId)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<BandLocationLeaderPeriodEmployee>> GetLocations(int? employeeId, string busniessId)
        {
            return await _context.BandLocationLeaderPeriodEmployees
                .Include(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location)
                .Where(e => e.EmployeeId == employeeId && e.BusinessId == busniessId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeType>> GetEmployeeTypesCompany(int companyID)
        {
            return await _context.BandLocationLeaderPeriodEmployees
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyID)
                .Select(e => e.Employee.Type)
                .ToListAsync();
                
        }
        public async Task<IEnumerable<EmployeeType>> GetEmployeeTypesLocation(int locationID)
        {
            return await _context.BandLocationLeaderPeriodEmployees
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationID)
                .Select(e => e.Employee.Type)
                .ToListAsync();

        }
        public async Task<IEnumerable<EmployeeType>> GetEmployeeTypesBand(int bandId)
        {
            return await _context.BandLocationLeaderPeriodEmployees
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId)
                .Select(e => e.Employee.Type)
                .ToListAsync();

        }

        public int GetBandLocationInnerReport(int? bandLocationId, int? typeId ,string businessId)
        {
            return _context.BandLocationLeaderPeriodEmployees
                .Where(e => e.BusinessId == businessId && !e.IsDeleted && e.BandLocationLeaderPeriod.BandLocationLeader.BandLocationId == bandLocationId)
                .Count(e => e.BandLocationLeaderPeriodEmployeePeriods.Any(e => e.BandLocationLeaderPeriodEmployee.Employee.TypeId == typeId));
        }
    }
}

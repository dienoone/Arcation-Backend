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
    public class BandLocationLeaderPeriodEmployeePeriodRepository : BaseRepository<BandLocationLeaderPeriodEmployeePeriod>, IBandLocationLeaderPeriodEmployeePeriodRepository
    {
        private readonly ApplicationDbContext _context;

        public BandLocationLeaderPeriodEmployeePeriodRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BandLocationLeaderPeriodEmployeePeriod> GetSubPeriodIncludeAttendace(int? bandLocationLeaderPeriodEmployeePeriodId, string BusinessId)
        {
            return await _context.BandLocationLeaderPeriodEmployeePeriods
                .Include(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.Leader)
                .Include(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Band)
                .Include(e => e.BandLocationLeaderPeriodEmployee.Employee.Type)
                .Include(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod)
                .Include(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Where(e => !e.IsDeleted))
                .Where(e => e.Id == bandLocationLeaderPeriodEmployeePeriodId && e.BusinessId == BusinessId && !e.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public double GetTotalCompanyPaied(int? companyId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriods
                .Where(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.PayiedValue);
        }

        public double GetTotalBandPaied(int? bandId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriods
                .Where(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId)
                .Sum(e => e.PayiedValue);
        }

        public double GetTotalLocationPaied(int? locationId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriods
                .Where(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId)
                .Sum(e => e.PayiedValue);
        }

        public double GetTotalCompanySalary(int? companyId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriods
                .Where(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.EmployeeSalary);
        }

        public double GetTotalBandSalary(int? bandId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriods
                .Where(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId)
                .Sum(e => e.EmployeeSalary);
        }

        public double GetTotalLocationSalary(int? locationId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriods
                .Where(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId)
                .Sum(e => e.EmployeeSalary);
        }

    }
}

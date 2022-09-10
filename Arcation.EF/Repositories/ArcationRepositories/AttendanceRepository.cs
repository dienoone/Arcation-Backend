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
    public class AttendanceRepository : BaseRepository<Attendance>, IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Attendance> GetInializeAttendance(int attendanceId)
        {
            return await _context.Attendances
                .Include(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Where(e => !e.IsDeleted))
                .Include(e => e.BandLocationLeaderPeriod)
                .ThenInclude(e => e.BandLocationLeaderPeriodEmployees.Where(e => !e.IsDeleted && e.State))
                .ThenInclude(e => e.Employee)
                .ThenInclude(e => e.Type)
                .Include(e => e.BandLocationLeaderPeriod.BandLocationLeader.Leader)
                .FirstOrDefaultAsync(e => e.Id == attendanceId);
        }
        public async Task<Attendance> TakeAttendance(int? attendanceId)
        {
            return await _context.Attendances
                .Include(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Where(e => !e.IsDeleted))
                .FirstOrDefaultAsync(e => e.Id == attendanceId);
        }

        public async Task<Attendance> GetAttendance(int? attendanceId, int? bandLocationLeaderPeriodId)
        {
            return await _context.Attendances
                .Include(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Where(e => !e.IsDeleted && e.BandLocationLeaderPeriodEmployeePeriod.State))
                .ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriod)
                .ThenInclude(e => e.BandLocationLeaderPeriodEmployee.Employee)
                .ThenInclude(e => e.Type)
                .Include(e => e.BandLocationLeaderPeriod)
                .Include(e => e.BandLocationLeaderPeriod.BandLocationLeader.Leader)
                .FirstOrDefaultAsync(e => e.Id == attendanceId && e.BandLocationLeaderPeriodId == bandLocationLeaderPeriodId);
        }

        public async Task<Attendance> GetSearchAttendance(int? attendanceId, string name, string businessID)
        {
            return await _context.Attendances
                .Include(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Where(e => !e.IsDeleted && e.BandLocationLeaderPeriodEmployeePeriod.State && e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.Employee.Name.Contains(name)))
                .ThenInclude(e => e.BandLocationLeaderPeriodEmployeePeriod)
                .ThenInclude(e => e.BandLocationLeaderPeriodEmployee.Employee)
                .ThenInclude(e => e.Type)
                .Include(e => e.BandLocationLeaderPeriod)
                .Include(e => e.BandLocationLeaderPeriod.BandLocationLeader.Leader)
                .FirstOrDefaultAsync(e => e.Id == attendanceId && e.BusinessId == businessID);
        }

        public double GetTotalCompanyBorrow(int? companyId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.BorrowValue);
        }

        public double GetTotalBandBorrow(int? bandId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId)
                .Sum(e => e.BorrowValue);
        }

        public double GetTotalLocationBorrow(int? locationId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId)
                .Sum(e => e.BorrowValue);
        }

        public double GetTotalCompanyHours(int? companyId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.WorkingHours);
        }

        public double GetTotalBandHours(int? bandId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId)
                .Sum(e => e.WorkingHours);
        }

        public double GetTotalLocationHours(int? locationId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId)
                .Sum(e => e.WorkingHours);
        }

        public double GetBandLocationInnerReport(int? bandLocationId, string businessId)
        {
            return  _context.Attendances
                .Where(e => e.BusinessId == businessId && !e.IsDeleted && e.BandLocationLeaderPeriod.BandLocationLeader.BandLocationId == bandLocationId)
                .Sum(e => e.WorkingHours * e.BandLocationLeaderPeriod.LeaderSalary);
        }

        public double GetBandLocationInnerReportPaied(int? bandLocationId, string businessId)
        {
            return _context.Attendances
                .Where(e => e.BusinessId == businessId && !e.IsDeleted && e.BandLocationLeaderPeriod.BandLocationLeader.BandLocationId == bandLocationId)
                .Sum(e => e.BorrowValue + e.BandLocationLeaderPeriod.TotalPaied);
        }

    }
}

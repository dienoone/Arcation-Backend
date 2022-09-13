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

        public double GetPeriodDaysReport(int? periodId, string businessId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.PeriodId == periodId && !e.IsDeleted && e.BusinessId == businessId)
                .Count();
        }

        public double GetTotalSalaryOfEmployeePeriodReoprt(int? periodId, string busniessId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.PeriodId == periodId && !e.IsDeleted && e.BusinessId == busniessId)
                .Sum(e => e.WorkingHours * e.BandLocationLeaderPeriod.LeaderSalary);
        }

        public double GetTotalPaiedOfEmployeePeriodReoprt(int? periodId, string busniessId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.PeriodId == periodId && !e.IsDeleted && e.BusinessId == busniessId)
                .Sum(e => e.BorrowValue + e.BandLocationLeaderPeriod.TotalPaied);
        }

        public double GetTotalBorrowOfEmployeePeriodReoprt(int? periodId, string busniessId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.PeriodId == periodId && !e.IsDeleted && e.BusinessId == busniessId)
                .Sum(e => e.BorrowValue);
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


        public double GetCompanyGlobalReportSalary(int? companyId, string busiessId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.WorkingHours * e.BandLocationLeaderPeriod.LeaderSalary);

        }
        public double GetLocationGlobalReportSalary(int? locationId, string busiessId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.WorkingHours * e.BandLocationLeaderPeriod.LeaderSalary);

        }
        public double GetBandGlobalReportSalary(int? bandId, string busiessId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.WorkingHours * e.BandLocationLeaderPeriod.LeaderSalary);

        }

        public double GetCompanyGlobalReportPaied(int? companyId, string busiessId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.BorrowValue + e.BandLocationLeaderPeriod.TotalPaied);

        }
        public double GetLocationGlobalReportPaied(int? locationId, string busiessId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.BorrowValue + e.BandLocationLeaderPeriod.TotalPaied);

        }
        public double GetBandGlobalReportPaied(int? bandId, string busiessId)
        {
            return _context.Attendances
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.BorrowValue + e.BandLocationLeaderPeriod.TotalPaied);

        }

    }
}

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
    public class BandLocationLeaderPeriodEmployeePeriodAttendanceRepository : BaseRepository<BandLocationLeaderPeriodEmployeePeriodAttendance>, IBandLocationLeaderPeriodEmployeePeriodAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public BandLocationLeaderPeriodEmployeePeriodAttendanceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public double GetTotalPaiedOfEmployeePeriodReoprt(int? periodId, string busniessId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.PeriodId == periodId && !e.IsDeleted && e.BusinessId == busniessId)
                .Sum(e => e.BorrowValue + e.BandLocationLeaderPeriodEmployeePeriod.PayiedValue);
        }

        public double GetTotalBorrowOfEmployeePeriodReoprt(int? periodId, string busniessId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.PeriodId == periodId && !e.IsDeleted && e.BusinessId == busniessId)
                .Sum(e => e.BorrowValue);
        }

        public double GetTotalSalaryOfEmployeePeriodReoprt(int? periodId, string busniessId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.PeriodId == periodId && !e.IsDeleted && e.BusinessId == busniessId)
                .Sum(e => e.WorkingHours * e.BandLocationLeaderPeriodEmployeePeriod.EmployeeSalary);
        }

        public double GetBandLocationInnerReport(int? bandLocationId, string businessId)
        {
            return  _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => !e.IsDeleted && e.BusinessId == businessId && e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocationId == bandLocationId)
                .Sum(e => e.WorkingHours * e.BandLocationLeaderPeriodEmployeePeriod.EmployeeSalary);
        }
        public double GetBandLocationInnerReportBorrow(int? bandLocationId, string businessId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => !e.IsDeleted && e.BusinessId == businessId && e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocationId == bandLocationId)
                .Sum(e => e.BorrowValue);
        }

        public double GetCompanyGlobalReportSalary(int? companyId, string busiessId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.WorkingHours * e.BandLocationLeaderPeriodEmployeePeriod.EmployeeSalary);

        }
        public double GetLocationGlobalReportSalary(int? locationId, string busiessId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.WorkingHours * e.BandLocationLeaderPeriodEmployeePeriod.EmployeeSalary);

        }
        public double GetBandGlobalReportSalary(int? bandId, string busiessId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.WorkingHours * e.BandLocationLeaderPeriodEmployeePeriod.EmployeeSalary);

        }

        public double GetCompanyGlobalReportPaied(int? companyId, string busiessId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.BorrowValue + e.BandLocationLeaderPeriodEmployeePeriod.PayiedValue);

        }
        public double GetLocationGlobalReportPaied(int? locationId, string busiessId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.BorrowValue + e.BandLocationLeaderPeriodEmployeePeriod.PayiedValue);

        }
        public double GetBandGlobalReportPaied(int? bandId, string busiessId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.BorrowValue + e.BandLocationLeaderPeriodEmployeePeriod.PayiedValue);

        }

    }
}

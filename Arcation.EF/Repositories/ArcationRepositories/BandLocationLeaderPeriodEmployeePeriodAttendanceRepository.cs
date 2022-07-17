﻿using Arcation.Core.Interfaces.ArcationInterfaces;
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

        public async Task<double> GetSumOfBorrowValue(IEnumerable<int> Ids)
        {
            double sum = 0;
            foreach(int id in Ids)
            {
                sum += await  _context.BandLocationLeaderPeriodEmployeePeriodAttendances.Where(e => !e.IsDeleted && e.AttendanceId == id).Select(e => e.BorrowValue).FirstOrDefaultAsync();
            }
            return sum;
        }
        public double GetTotalCompanyBorrow(int? companyId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.BorrowValue);
        }

        public double GetTotalBandBorrow(int? bandId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId)
                .Sum(e => e.BorrowValue);
        }

        public double GetTotalLocationBorrow(int? locationId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId)
                .Sum(e => e.BorrowValue);
        }

        public double GetTotalCompanyHours(int? companyId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.WorkingHours);
        }

        public double GetTotalBandHours(int? bandId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId)
                .Sum(e => e.WorkingHours);
        }

        public double GetTotalLocationHours(int? locationId)
        {
            return _context.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Where(e => e.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId)
                .Sum(e => e.WorkingHours);
        }
    }
}

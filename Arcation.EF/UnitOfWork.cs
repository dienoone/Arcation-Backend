using Arcation.Core;
using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.EF.Repositories.ArcationRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ICompanyRepository Companies { get; private set; }
        public IBandRepository Bands { get; private set; }
        public ILocationRepository Locations { get; private set; }
        public IBandLocationRepository BandLocations { get; private set; }
        public IBillRepository Bills { get; private set; }
        public IBLWestedRepository BLWesteds { get; private set; }
        public IIncomeRepository Incomes { get; private set; }
        public ILeaderRepository Leaders { get; private set; }
        public IPeriodRepository Periods { get; private set; }
        public ITransactionRepository LeaderTransactions { get; private set; }
        public IWestedRepository LeaderWesteds { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public IEmployeeTypeRepository EmployeeTypes { get; private set; }
        public IAttendanceRepository Attendances { get; private set; }
        public IBandLocationLeaderRepository BandLocationLeaders { get; private set; }
        public IBandLocationLeaderPeriodRepository BandLocationLeaderPeriods { get; private set; }
        public IBandLocationLeaderPeriodEmployeeRepository BandLocationLeaderPeriodEmployees { get; private set; }
        public IBandLocationLeaderPeriodEmployeePeriodRepository BandLocationLeaderPeriodEmployeePeriods { get; private set; }
        public IBandLocationLeaderPeriodEmployeePeriodAttendanceRepository BandLocationLeaderPeriodEmployeePeriodAttendances { get; private set; }
        public IToolRepository Tools { get; private set; }
        public IExtractRepository Extracts { get; private set; }
        public IExtractRowRepository ExtractRows { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Companies = new CompanyRepository(_context);
            Bands = new BandRepository(_context);
            Locations = new LocationRepository(_context);
            BandLocations = new BandLocationRepository(_context);
            Bills = new BillRepository(_context);
            BLWesteds = new BLWestedRepository(_context);
            Incomes = new IncomeRepository(_context);
            Leaders = new LeaderRepository(_context);
            Periods = new PeriodRepository(_context);
            LeaderWesteds = new WestedRepository(_context);
            LeaderTransactions = new TransactionRepository(_context);
            Employees = new EmployeeRepository(_context);
            EmployeeTypes = new EmployeeTypeRepository(_context);
            Attendances = new AttendanceRepository(_context);
            Tools = new ToolRepository(_context);
            Extracts = new ExtractRepository(_context);
            ExtractRows = new ExtractRowRepository(_context);
            BandLocationLeaders = new BandLocationLeaderRepository(_context);
            BandLocationLeaderPeriods = new BandLocationLeaderPeriodRepository(_context);
            BandLocationLeaderPeriodEmployees = new BandLocationLeaderPeriodEmployeeRepository(_context);
            BandLocationLeaderPeriodEmployeePeriods = new BandLocationLeaderPeriodEmployeePeriodRepository(_context);
            BandLocationLeaderPeriodEmployeePeriodAttendances = new BandLocationLeaderPeriodEmployeePeriodAttendanceRepository(_context);
        }

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

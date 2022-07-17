using Arcation.Core.Interfaces.ArcationInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core
{
    public interface IUnitOfWork : IDisposable
    {

        ICompanyRepository Companies { get; }
        IBandRepository Bands { get; }
        ILocationRepository Locations { get; }
        IBandLocationRepository BandLocations { get; }
        IBillRepository Bills { get; }
        IBLWestedRepository BLWesteds { get; }
        IIncomeRepository Incomes { get; }
        ILeaderRepository Leaders { get; }
        IPeriodRepository Periods { get; }
        ITransactionRepository LeaderTransactions { get; }
        IWestedRepository LeaderWesteds { get; }
        IEmployeeRepository Employees { get; }
        IEmployeeTypeRepository EmployeeTypes { get; }
        IAttendanceRepository Attendances { get; }
        IBandLocationLeaderRepository BandLocationLeaders { get; }
        IBandLocationLeaderPeriodRepository BandLocationLeaderPeriods { get; }
        IBandLocationLeaderPeriodEmployeeRepository BandLocationLeaderPeriodEmployees { get; }
        IBandLocationLeaderPeriodEmployeePeriodRepository BandLocationLeaderPeriodEmployeePeriods { get; }
        IBandLocationLeaderPeriodEmployeePeriodAttendanceRepository BandLocationLeaderPeriodEmployeePeriodAttendances { get; }
        IToolRepository Tools { get; }
        IExtractRepository Extracts { get; }
        IExtractRowRepository ExtractRows { get; }

        Task<bool> Complete();
    }
}

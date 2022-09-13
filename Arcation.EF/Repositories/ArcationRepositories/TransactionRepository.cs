using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF.Repositories.ArcationRepositories
{
    public class TransactionRepository : BaseRepository<Transiction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public double GetSum(int? bandLocationLeaderPeriodId)
        {
            return _context.Transactions.Where(e => !e.IsDeleted).Sum(e => e.Value);
        }


        public double GetPeriodReport(int? periodId, string businessId)
        {
            return _context.Transactions
                .Where(e => e.PeriodLeader.PeriodId == periodId && e.BusinessId == businessId && !e.IsDeleted)
                .Sum(e => e.Value);
        }

        public double GetBandLocationInnerReport(int? bandLocationId, string businessId)
        {
            return _context.Transactions
                .Where(e => e.PeriodLeader.BandLocationLeader.BandLocationId == bandLocationId && e.BusinessId == businessId && !e.IsDeleted)
                .Sum(e => e.Value);
        }

        public double GetCompanyGlobalReport(int? companyId, string busiessId)
        {
            return _context.Transactions
                .Where(e => e.PeriodLeader.BandLocationLeader.BandLocation.Location.CompanyId == companyId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.Value);

        }

        public double GetLocationGlobalReport(int? locationId, string busiessId)
        {
            return _context.Transactions
                .Where(e => e.PeriodLeader.BandLocationLeader.BandLocation.LocationId == locationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.Value);

        }

        public double GetBandGlobalReport(int? bandLocationId, string busiessId)
        {
            return _context.Transactions
                .Where(e => e.PeriodLeader.BandLocationLeader.BandLocation.Id == bandLocationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.Value);

        }
    }
}

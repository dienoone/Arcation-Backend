using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF.Repositories.ArcationRepositories
{
    public class WestedRepository : BaseRepository<Wested>, IWestedRepository
    {
        private readonly ApplicationDbContext _context;
        public WestedRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        public double GetSum(int? bandLocationLeaderPeriodId)
        {
            return _context.Westeds.Where(e => !e.IsDeleted).Sum(e => e.Value);
        }

        public double GetTotalCompany(int? companyId)
        {
            return _context.Westeds
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.Value);
        }

        public double GetTotalBand(int? bandId)
        {
            return _context.Westeds
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandId)
                .Sum(e => e.Value);
        }

        public double GetTotalLocation(int? locationId)
        {
            return _context.Westeds
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId)
                .Sum(e => e.Value);
        }

        public double GetBandLocationInnerReport(int? bandLocationId, string businessId)
        {
            return _context.Westeds
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocationId == bandLocationId && e.BusinessId == businessId && !e.IsDeleted)
                .Sum(e => e.Value);
        }

        public double GetCompanyGlobalReport(int? companyId, string busiessId)
        {
            return _context.Westeds
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.CompanyId == companyId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.Value);

        }

        public double GetLocationGlobalReport(int? locationId, string busiessId)
        {
            return _context.Westeds
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.LocationId == locationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.Value);

        }

        public double GetBandGlobalReport(int? bandLocationId, string busiessId)
        {
            return _context.Westeds
                .Where(e => e.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Id == bandLocationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.Value);

        }
    }
}

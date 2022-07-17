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
    }
}

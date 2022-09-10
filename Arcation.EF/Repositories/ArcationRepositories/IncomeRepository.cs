using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF.Repositories.ArcationRepositories
{
    public class IncomeRepository  : BaseRepository<Income>, IIncomeRepository
    {
        private readonly ApplicationDbContext _context;
        public IncomeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public double GetTotalCompany(int? companyId)
        {
            return _context.Incomes
                .Where(e => e.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.Price);
        }

        public double GetTotalBand(int? bandId)
        {
            return _context.Incomes
                .Where(e => e.BandLocation.Id == bandId)
                .Sum(e => e.Price);
        }

        public double GetTotalLocation(int? locationId)
        {
            return _context.Incomes
                .Where(e => e.BandLocation.LocationId == locationId)
                .Sum(e => e.Price);
        }

        public double GetBandLocationInnerReport(int? bandLocationId, string busiessId)
        {
            return _context.Incomes
                .Where(e => e.BandLocationId == bandLocationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.Price);

        }
    }
}

using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF.Repositories.ArcationRepositories
{
    public class BillRepository : BaseRepository<Bill>, IBillRepository
    {
        private readonly ApplicationDbContext _context;
        public BillRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public double GetTotalCompany(int? companyId)
        {
            return _context.Bills
                .Where(e => e.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.BillPrice);
        }

        public double GetTotalBand(int? bandId)
        {
            return _context.Bills
                .Where(e => e.BandLocation.Id == bandId)
                .Sum(e => e.BillPrice);
        }

        public double GetTotalLocation(int? locationId)
        {
            return _context.Bills
                .Where(e => e.BandLocation.LocationId == locationId)
                .Sum(e => e.BillPrice);
        }


        public double GetBandLocationInnerReport(int? bandLocationId, string busiessId)
        {
            return _context.Bills
                .Where(e => e.BandLocationId == bandLocationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.BillPrice);
            
        }
    }
}

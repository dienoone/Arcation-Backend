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
    public class ExtractRepository : BaseRepository<Extract>, IExtractRepository
    {
        private readonly ApplicationDbContext _context;

        public ExtractRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Extract>> GetExtractsAsync(int? bandLocationId, string businessId)
        {
            return await _context.Extracts
                .Where(e => e.BandLocationId == bandLocationId && e.BusinessId == businessId)
                .ToListAsync();
        }

        public async Task<Extract> GetExtractAsync(int? extractId, string businessId)
        {
            return await _context.Extracts
                .FirstOrDefaultAsync(e => e.ExtractId == extractId && e.BusinessId == businessId);
        }

        public async Task<IEnumerable<Extract>> SearchExtractAsync(int? bandLocationId, string name, string businessId)
        {
            return await _context.Extracts
                .Where(e => e.BandLocationId == bandLocationId && e.BusinessId == businessId && !e.IsDeleted && e.ExtractName.Contains(name))
                .ToListAsync();
        }


        public double GetBandLocationInnerReport(int? bandLocationId, string busiessId)
        {
            return _context.Extracts
                .Where(e => e.BandLocationId == bandLocationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.TotalPrice);

        }

        public double GetCompanyGlobalReport(int? companyId, string busiessId)
        {
            return _context.Extracts
                .Where(e => e.BandLocation.Location.CompanyId == companyId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.TotalPrice);

        }

        public double GetLocationGlobalReport(int? locationId, string busiessId)
        {
            return _context.Extracts
                .Where(e => e.BandLocation.LocationId == locationId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.TotalPrice);

        }

        public double GetBandGlobalReport(int? bandId, string busiessId)
        {
            return _context.Extracts
                .Where(e => e.BandLocation.Id == bandId && !e.IsDeleted && e.BusinessId == busiessId)
                .Sum(e => e.TotalPrice);

        }
    }
}

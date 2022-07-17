using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF.Repositories.ArcationRepositories
{
    public class ExtractRowRepository : BaseRepository<ExtractRow>, IExtractRowRepository
    {
        private readonly ApplicationDbContext _context;
        public ExtractRowRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public double GetTotalCompany(int? companyId)
        {
            return _context.ExtractRows
                .Where(e => e.Extract.BandLocation.Location.CompanyId == companyId)
                .Sum(e => e.TotalPrice);
        }

        public double GetTotalBand(int? bandId)
        {
            return _context.ExtractRows
                .Where(e => e.Extract.BandLocation.Id == bandId)
                .Sum(e => e.TotalPrice);
        }

        public double GetTotalLocation(int? locationId)
        {
            return _context.ExtractRows
                .Where(e => e.Extract.BandLocation.LocationId == locationId)
                .Sum(e => e.TotalPrice);
        }
    }
}

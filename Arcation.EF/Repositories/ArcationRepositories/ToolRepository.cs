using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF.Repositories.ArcationRepositories
{
    public class ToolRepository : BaseRepository<Tool>, IToolRepository
    {
        private readonly ApplicationDbContext _context;
        public ToolRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

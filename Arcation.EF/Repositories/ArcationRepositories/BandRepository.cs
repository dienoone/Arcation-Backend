using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF.Repositories.ArcationRepositories
{
    public class BandRepository : BaseRepository<Band>, IBandRepository
    {
        private readonly ApplicationDbContext _context;

        public BandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class BLWested : AuditModel
    {
        public int Id { get; set; }
        public string Estatement { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string Note { get; set; }
        public int BandLocationId { get; set; }
        public BandLocation BandLocation { get; set; }
    }
}

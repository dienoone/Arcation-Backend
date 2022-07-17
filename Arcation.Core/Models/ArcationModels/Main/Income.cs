using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Income : AuditModel
    {
        public int IncomeId { get; set; }
        public string Estatement { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public double Price { get; set; }
        public int BandLocationId { get; set; }
        public BandLocation BandLocation { get; set; }
    }
}

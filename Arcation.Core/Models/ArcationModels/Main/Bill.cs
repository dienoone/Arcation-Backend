using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Bill : AuditModel
    {
        public int BillId { get; set; }
        public string BillCode { get; set; }
        public DateTime BillDate { get; set; }
        public string BillPhoto { get; set; }
        public double BillPrice { get; set; }
        public string BillNote { get; set; }
        public int BandLocationId { get; set; }
        public BandLocation BandLocation { get; set; }
    }
}

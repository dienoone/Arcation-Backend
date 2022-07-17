using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class ExtractRow : AuditModel
    {
        public int ExtractRowId { get; set; }
        public string Estatement { get; set; }
        public string EstatementUnite { get; set; }
        public double EstatementUnitePrice { get; set; }
        public double Quantity { get; set; }
        public double Percentage { get; set; }
        public double TotalPrice { get; set; }
        public string Note { get; set; }
        public int ExtractId { get; set; }
        public Extract Extract { get; set; }

    }
}

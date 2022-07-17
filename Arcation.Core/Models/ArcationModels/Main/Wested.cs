using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Wested : AuditModel
    {
        public int Id { get; set; }
        public string Estatement { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public string Note { get; set; }

        // Forign Key:
        public int BandLocationLeaderPeriodId { get; set; }
        public BandLocationLeaderPeriod BandLocationLeaderPeriod { get; set; }
    }
}

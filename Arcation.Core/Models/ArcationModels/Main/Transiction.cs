using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Transiction : AuditModel
    {
        public int Id { get; set; }
        public string Estatement { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

        // Forign Key:
        public int BandLocationLeaderPeriodId { get; set; }
        public BandLocationLeaderPeriod PeriodLeader { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class AddLeaderWestedDto : UpdateLeaderWestedDto
    {
        [Required]
        public int BandLocationLeaderPeriodId { get; set; }
    }
    public class UpdateLeaderWestedDto
    {
        [Required]
        public string Estatement { get; set; }
        [Required]
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

    }
    public class LeaderWestedDto
    {
        public int Id { get; set; }
        public string Estatement { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public string Note { get; set; }
        public int BandLocationLeaderPeriodId { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class TransictionDto
    {
        public int Id { get; set; }
        public string Estatement { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public int BandLocationLeaderPeriodId { get; set; }
    }

    public class AddTransictionDto : UpdateTransictionDto
    {
        [Required]
        public int BandLocationLeaderPeriodId { get; set; }
    }

    public class UpdateTransictionDto
    {
        [Required]
        public string Estatement { get; set; }
        [Required]
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
    }
}

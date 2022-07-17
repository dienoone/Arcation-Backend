using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class IncomeDto
    {
        public int IncomeId { get; set; }
        public string Estatement { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public double Price { get; set; }
        public int BandLocationId { get; set; }
    }

    public class AddIncomeDto
    {
        [Required]
        public string Estatement { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Note { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int BandLocationId { get; set; }

    }

    public class UpdateIncomeDto
    {
        [Required]
        public string Estatement { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Note { get; set; }
        [Required]
        public double Price { get; set; }
    }
}

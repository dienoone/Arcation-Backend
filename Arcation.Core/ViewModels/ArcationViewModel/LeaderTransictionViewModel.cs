using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class AddLeaderTransictionDto : UpdateLeaderTransictionDto
    {
        [Required]
        // Forign Key:
        public int PeriodLeaderId { get; set; }
    }
    public class UpdateLeaderTransictionDto
    {
        [Required]
        public string Estatement { get; set; }
        [Required]
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

    }

    public class LeaderTransictionDto
    {
        public int Id { get; set; }
        public string Estatement { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

        // Forign Key:
        public int PeriodLeaderId { get; set; }
    }

    
}

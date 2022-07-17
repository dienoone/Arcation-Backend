using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class AddExtract 
    { 
        [Required]
        public string ExtractName { get; set; }
    }
    public class UpdateExtract : AddExtract { }

    public class AddExtractRow 
    {
        [Required]
        public string Estatement { get; set; }
        [Required]
        public string EstatementUnite { get; set; }
        [Required]
        public double EstatementUnitePrice { get; set; }
        [Required]
        public double Quantity { get; set; }
        [Required]
        public double Percentage { get; set; }
        public string Note { get; set; }
    }
    public class UpdateExtractRow : AddExtractRow { }


    public class AllExtracts 
    {
        public int ExtractId { get; set; }
        public string ExtractName { get; set; }
        public double TotalPrice { get; set; }
        public IEnumerable<ExtactRowsDto> ExtactRows { get; set; }

    }
    public class ExtactRowsDto 
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
    }


}

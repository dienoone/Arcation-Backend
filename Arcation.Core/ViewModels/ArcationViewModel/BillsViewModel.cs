using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class BillDto
    {
        public int BillId { get; set; }
        public string BillCode { get; set; }
        public DateTime BillDate { get; set; }
        public string BillPhoto { get; set; }
        public double BillPrice { get; set; }
        public string BillNote { get; set; }
        public int BandLocationId { get; set; }
    }

    public class AddBillDto : UpdateBillDto
    {

        [Required]
        public int BandLocationId { get; set; }

    }

    public class UpdateBillDto
    {
        [Required]
        public string BillCode { get; set; }
        public DateTime BillDate { get; set; }
        public IFormFile BillPhoto { get; set; }
        [Required]
        public double BillPrice { get; set; }
        public string BillNote { get; set; }
    }



}

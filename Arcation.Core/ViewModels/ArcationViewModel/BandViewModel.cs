using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class BandViewModel
    {
        public int Id { get; set; }
        public string BandName { get; set; }

    }

    public class AddBandViewModel 
    {
        [Required]
        public string BandName { get; set; }

    }

    public class UpdateBandViewModel : AddBandViewModel
    {
    }


}

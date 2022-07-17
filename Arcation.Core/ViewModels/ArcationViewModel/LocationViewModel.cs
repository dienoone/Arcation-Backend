using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public bool LocationState { get; set; }

        public List<BandLocationDto> Bands { get; set; }

    }

    public class BandLocationDto
    {
        public int BandLocationId { get; set; }
        public int BandId { get; set; }
        public string BandName { get; set; }
    }

    public class AddLocationViewModel
    {
        [Required]
        public string LocationName { get; set; }
        public DateTime StartingDate { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public List<int> bandIds { get; set; }

    }

    public class UpdateLocationViewModel : AddLocationViewModel
    {
        public DateTime? EndingDate { get; set; }
    }


}

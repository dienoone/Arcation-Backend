using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Arcation.Core.Models.ArcationModels.Main
{
    public class Location : AuditModel
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartingDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndingDate { get; set; }
        public bool LocationState { get; set; }
        public int CompanyId { get; set; }


        // Referance Properties
        public Company Company { get; set; }
        public List<BandLocation> BandLocations { get; set; }
    }
}

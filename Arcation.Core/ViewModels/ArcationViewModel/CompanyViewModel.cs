using Arcation.Core.Models.ArcationModels.Main;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels.ArcationViewModel
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }

    }

    public class UpdateCompanyViewModel : AddCompanyViewModel
    {
    }

    public class AddCompanyViewModel
    {
        [Required]
        public string Name { get; set; }
        public IFormFile Photo { get; set; }

    }




}

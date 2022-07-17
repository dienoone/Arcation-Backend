using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Models
{
    public class GoogleUserRequest
    {
        public const string PROVIDER = "google";
        [JsonProperty("idToken")]
        [Required]
        public string IdToken { get; set; }
    }
}

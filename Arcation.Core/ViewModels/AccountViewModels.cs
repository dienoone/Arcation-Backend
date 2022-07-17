using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Arcation.Core.ViewModels
{
    public class LoginResponseViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpireOn { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        [JsonIgnore]
        public IEnumerable<string> Errors { get; set; }
        [JsonIgnore]
        public string Message { get; set; }
        [JsonIgnore]
        public bool IsAuthenticated { get; set; }

    }

    public class RegisterViewModel
    {
        [Required]
        public string FristName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        /*        [Required]
                [StringLength(50)]
                public string PhoneNumber { get; set; }*/

        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
    }

    public class RefreshTokenViewModel
    {
        [Required]
        public string refreshToken { get; set; }
    }

    public class RevokeTokenViewModel
    {
        [Required]
        public string Token { get; set; }

    }

    public class ForgetPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }

    public class ConfirmTokenFrogetPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        public string AccessToken { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(50, MinimumLength = 5)]
        [Compare("ConfirmPassword")]
        public string NewPassword { get; set; }
        [Required, StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }

    }

    public class AddRoleModel
    {
        [Required]
        public string UserId { get; set; }
        public string Role { get; set; }
    }

    public class AccountViewModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

}

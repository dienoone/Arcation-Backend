using Arcation.Core.Interfaces;
using Arcation.Core.Models;
using Arcation.Core.ViewModels;
using Arcation.Core.ViewModels.ArcationViewModel;
using Arcation.EF.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Arcation.EF.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMailingServices _mailingServices;
        private readonly FourDigitTokenProvider _fourDigit;

        // Constructor:
        public AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
           IMailingServices mailingServices, IConfiguration configuration, FourDigitTokenProvider fourDigit)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mailingServices = mailingServices;
            _configuration = configuration;
            _fourDigit = fourDigit;
        }


        
        // Login By Email Or UserName:
        public async Task<LoginResponseViewModel> LoginAsync(LoginViewModel model)
        {
            var response = new LoginResponseViewModel();

            var user =  await _userManager.FindByEmailAsync(model.UserName) ?? await _userManager.FindByNameAsync(model.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                response.Message = "UserName Or Password is incorrect !!";
                return response;
            }
            else
            {
                if (user.IsActive)
                {
                    var jwtToken = await CreateJwtToken(user);
                    var roles = await _userManager.GetRolesAsync(user);

                    response.IsAuthenticated = true;
                    response.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                    response.UserName = user.UserName;
                    response.Email = user.Email;
                    response.ExpireOn = jwtToken.ValidTo;
                    response.Roles = roles.ToList();

                    if (user.RefreshTokens.Any(t => t.IsActive))
                    {
                        var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                        response.RefreshToken = activeToken.Token;
                        response.RefreshTokenExpiration = activeToken.ExpiresOn;
                    }
                    else
                    {
                        var refreshToken = GenerateRefreshToken();
                        response.RefreshToken = refreshToken.Token;
                        response.RefreshTokenExpiration = refreshToken.ExpiresOn;
                        user.RefreshTokens.Add(refreshToken);

                        await _userManager.UpdateAsync(user);
                    }

                    return response;
                }
                response.IsAuthenticated = false;
                return response;
            }

        }

        public async Task<LoginResponseViewModel> RefreshTokenAsync(string token)
        {
            var response = new LoginResponseViewModel();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                response.IsAuthenticated = false;
                response.Message = "Invalid Token";
                return response;
            }
            else
            {
                var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

                if (!refreshToken.IsActive)
                {
                    response.IsAuthenticated = false;
                    response.Message = "Invalid Token";
                    return response;
                }

                refreshToken.RevokedOn = DateTime.UtcNow;

                var newRefreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);

                var jwtToken = await CreateJwtToken(user);
                var roles = await _userManager.GetRolesAsync(user);

                response.Email = user.Email;
                response.UserName = user.UserName;
                response.Roles = roles.ToList();
                response.IsAuthenticated = true;
                response.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                response.RefreshToken = newRefreshToken.Token;
                response.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

                return response;
            }

        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            // Search for user:
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user != null)
            {
                var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
                if (refreshToken.IsActive)
                {
                    refreshToken.RevokedOn = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                    return true;
                }
                return false;
            }           
            return false;

        }

        public async Task<LoginResponseViewModel> GenerateForgetPasswordTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await SendEmailConfirmation(user);
                return new LoginResponseViewModel { IsAuthenticated = true };                
            }
            return new LoginResponseViewModel { IsAuthenticated = false };
        }

        public async Task<LoginResponseViewModel> ConfrimPasswordToken(ConfirmTokenFrogetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if(await ConfirmResetPassword(user, model.Token))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    return new LoginResponseViewModel
                    {
                        IsAuthenticated = true,
                        Email = model.Email,
                        AccessToken = token
                    };
                }
                return new LoginResponseViewModel { IsAuthenticated = false };
            }
            return new LoginResponseViewModel { IsAuthenticated = false };
        }

        public async Task<LoginResponseViewModel> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, model.AccessToken, model.NewPassword);
                if (result.Succeeded)
                {
                    var response = new LoginResponseViewModel();

                    if (user.RefreshTokens.Any(t => t.IsActive))
                    {
                        var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                        response.RefreshToken = activeToken.Token;
                        response.RefreshTokenExpiration = activeToken.ExpiresOn;
                    }
                    else
                    {
                        var refreshToken = GenerateRefreshToken();
                        response.RefreshToken = refreshToken.Token;
                        response.RefreshTokenExpiration = refreshToken.ExpiresOn;
                        user.RefreshTokens.Add(refreshToken);

                        await _userManager.UpdateAsync(user);
                    }

                    var accessToken = await CreateJwtToken(user);
                    var roles = await _userManager.GetRolesAsync(user);

                    response.IsAuthenticated = true;
                    response.AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
                    response.UserName = user.UserName;
                    response.Email = user.Email;
                    response.ExpireOn = accessToken.ValidTo;
                    response.Roles = roles.ToList();

                    return response;
                }
                return new LoginResponseViewModel { IsAuthenticated = false };
            }
            return new LoginResponseViewModel { IsAuthenticated = false };
        }

        // Reginster For Admin:
        public async Task<LoginResponseViewModel> RegisterLeaderAsync(AddLeaderDto model)
        {
            var response = new LoginResponseViewModel();
            // Check If UserName Exist:
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                response.IsAuthenticated = false;
                response.Message = "UserName Is Aleardy Registered!";
                return response;
            }

            // Create New User 
            var user = new ApplicationUser { UserName = model.UserName, BusinessId = model.BusinessId , FirstName = model.FristName, LastName = model.LastName, Email = model.UserName, IsActive = true};

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                response.IsAuthenticated = false;
                response.Message = errors;
                return response;
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "User");

                response.IsAuthenticated = true;
                response.Message = user.Id;
                return response;
            }

        }


        #region TODO:

        // Reginster For Admin:
        public async Task<LoginResponseViewModel> RegisterAsync(RegisterViewModel model)
        {
            // Check If UserName Or Email Is Exist:
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new LoginResponseViewModel { Message = "Email Is Aleardy Registered!" };

            if (await _userManager.FindByNameAsync(model.UserName) != null)
                return new LoginResponseViewModel { Message = "UserName Is Aleardy Registered!" };


            // Create New User 
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.UserName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new LoginResponseViewModel { Message = errors };
            }
            else
            {

                user.BusinessId = user.Id;
                await _userManager.AddToRoleAsync(user, "Admin");

                await SendEmailConfirmation(user);

                var refreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);

                await _userManager.UpdateAsync(user);

                var JwtToken = await CreateJwtToken(user);

                return new LoginResponseViewModel
                {
                    Email = model.Email,
                    IsAuthenticated = true,
                    ExpireOn = JwtToken.ValidTo,
                    Roles = new List<string> { "Admin" },
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(JwtToken)
                };
            }

        }

        // Google:
        public async Task<LoginResponseViewModel> AuthenticateGoogleUserAsync(GoogleUserRequest request)
        {
            Payload payload = await ValidateAsync(request.IdToken, new ValidationSettings
            {
                Audience = new[] { _configuration["Authentication:Google:ClientId"] }
            });

            var user = await GetOrCreateExternalLoginUser(GoogleUserRequest.PROVIDER, payload.Subject, payload.Email, payload.GivenName, payload.FamilyName);

            if (user == null)
            {
                return new LoginResponseViewModel
                {
                    IsAuthenticated = false,
                    Message = "Some Thing Goes Wrong"
                };
            }
            else
            {
                await SendEmailConfirmation(user);
                await _userManager.AddToRoleAsync(user, "Admin");

                var JwtToken = await CreateJwtToken(user);

                return new LoginResponseViewModel
                {
                    Email = payload.Email,
                    IsAuthenticated = true,
                    //  ExpireOn = JwtToken.ValidTo,
                    Roles = new List<string> { "Admin" },
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(JwtToken)
                };
            }
        }

        #endregion

        #region Helper

        #region TOKEN:

        // This Function To Create Tokens:
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();


            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id),
                new Claim("bid", user.BusinessId)

            }.Union(userClaims).Union(roleClaims);

            var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:JWT:Secret"]));
            var signingCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Authentication:JWT:ValidIssuer"],
                audience: _configuration["Authentication:JWT:ValidAudience"],
                claims: Claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Authentication:JWT:DuraionInDays"])),
                signingCredentials: signingCredentials
            );

            return token;
        }

        // Finish : 
        private Core.Models.RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new Core.Models.RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };

        }


        #endregion

        #region EMAIL SERVICES:

        // This Method To Send Confirmation Email: => ConrimEmailAsync:
        private async Task SendEmailConfirmation(ApplicationUser user)
        {
            var code = await _fourDigit.GenerateAsync(FourDigitTokenProvider.FourDigitEmail, _userManager, user);
            await _mailingServices.SendEmailAsync(user.Email, "Confirm Your Email", "<h1>Welcom To Arcation</h1>"
                + $"<p>Please Confirm your Email by Entring This Code:  {code}</p>");
        }

        // Confirm Email:
        public async Task<LoginResponseViewModel> ConfirmEmailAsync(string userID, string token)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return new LoginResponseViewModel
                {
                    IsAuthenticated = false,
                    Message = "User not found"
                };
            }


            var result = await _fourDigit.ValidateAsync(FourDigitTokenProvider.FourDigitEmail, token, _userManager, user);

            if (result)
            {
                return new LoginResponseViewModel
                {
                    Message = "Email confirmed successfully",
                    IsAuthenticated = true
                };
            }

            return new LoginResponseViewModel
            {
                IsAuthenticated = false,
                Message = "Email did not confirmed",
                //Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<bool> ConfirmResetPassword(ApplicationUser user, string token)
        {
            return await _fourDigit.ValidateAsync(FourDigitTokenProvider.FourDigitEmail, token, _userManager, user);
        }

        #endregion

        #region PHONE SERVICES:

        private async Task SendPhoneConfirmation(ApplicationUser user, string phoneNumber)
        {

            var confimPhoneToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            // TODO: SendPhone SMS:
        }

        public async Task<LoginResponseViewModel> ConfirmPhoneAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new LoginResponseViewModel
                {
                    IsAuthenticated = false,
                    Message = "User not found"
                };
            }

            var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, token);

            if (!result.Succeeded)
            {
                return new LoginResponseViewModel
                {

                    IsAuthenticated = false,
                    Message = "Phone did not confirmed",
                    Errors = result.Errors.Select(e => e.Description)

                };
            }
            else
            {

                return new LoginResponseViewModel
                {
                    Message = "Phone confirmed successfully",
                    IsAuthenticated = true
                };
            }


        }

        #endregion

        #region TODO:
        // Add User To Role: User Or Admin
        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null || !await _roleManager.RoleExistsAsync(model.Role))
                return "In Valid User Id Or Role Name";

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User Alread Asigned to this Role";

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "some This Goes Wrong";

        }

        // Google:
        private async Task<ApplicationUser> GetOrCreateExternalLoginUser(string provider, string key, string email, string firstName, string lastName)
        {
            var user = await _userManager.FindByLoginAsync(provider, key);

            if (user != null) return user;

            user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Id = key,
                };
                await _userManager.CreateAsync(user);
            }

            var info = new UserLoginInfo(provider, key, provider.ToUpperInvariant());

            var result = await _userManager.AddLoginAsync(user, info);

            if (result.Succeeded) return user;

            return null;

        }
        #endregion

        #endregion

        #region Setting:
        public async Task<AccountViewModel> GetUserDetail(string userId)
        {
            AccountViewModel accountViewModel = new();
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                accountViewModel.Email = user.Email;
                accountViewModel.Name = user.FirstName + " " + user.LastName;
                accountViewModel.PhoneNumber = user.PhoneNumber;
                accountViewModel.UserName = user.UserName;
                
                return accountViewModel;
            }
            return null;
        }
        #endregion

    }
}

using Arcation.Core.Interfaces;
using Arcation.Core.Models;
using Arcation.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Arcation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMailingServices _mailingServices;

        public AccountController(IAccountRepository accountRepository, IMailingServices mailingServices)
        {
            _accountRepository = accountRepository;
            _mailingServices = mailingServices;
        }

        // api/account/login: => Done:
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.LoginAsync(model);

                if (result.IsAuthenticated)
                {
                    // TODO: Email 
                    if (model.UserName.Contains("@gmail.com"))
                    {

                        await _mailingServices.SendEmailAsync(model.UserName, "New Login To Arcation",
                            "<h1>Hey!, New Login To Your Account Noticed</h1>" +
                            "<p>New Login, To Your Account At " + DateTime.Now + "</p>");

                    }
                    return Ok(result); // Status Code: 200 OK
                }
                return BadRequest(result.Message); // Status Code: 400 BadRequest!!
            }
            return BadRequest(ModelState); // Status Code: 400 BadRequest!!
        }

        // api/account/refreshToken => Done:
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.RefreshTokenAsync(model.refreshToken);
                if (result.IsAuthenticated)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest();
        }

        // api/account/revokeToken => Done:
        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Token))
                {
                    var result = await _accountRepository.RevokeTokenAsync(model.Token);
                    if (result)
                    {
                        return Ok();
                    }
                    return BadRequest("Token Is Invalid");
                }
                return BadRequest("Token Is Required");
            }
            return BadRequest();
        }

        // api/account/forgetpassword
        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromBody]ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _accountRepository.GenerateForgetPasswordTokenAsync(model.Email);

                if (result.IsAuthenticated)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest();
        }

        // api/account/confirmPasswordToken
        [HttpPost("ConfirmPasswordToken")]
        public async Task<IActionResult> ConfirmPasswordToken([FromBody] ConfirmTokenFrogetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Email)) return NotFound();

                var result = await _accountRepository.ConfrimPasswordToken(model);

                if (result.IsAuthenticated)
                {
                    return Ok(new { Email = result.Email, AccessToken = result.AccessToken });
                }
                return BadRequest();
            }
            return BadRequest();
        }

        // api/account/resetpassword
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resutl = await _accountRepository.ResetPasswordAsync(model);
                if (resutl.IsAuthenticated)
                {
                    return Ok(resutl);
                }
                return BadRequest();
            }
            return BadRequest();
        }

        #region TODO:

        // api/account/register:
        //[Authorize(Roles ="SuperAdmin")]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.RegisterAsync(model);

                if (result.IsAuthenticated)
                {
                    return Ok(result); // Status Code: 200 OK
                }
                return BadRequest(result.Message); // Status Code: 400 BadRequest!!
            }
            return BadRequest(ModelState); // Status Code: 400 BadRequest!!
        }

        // api/account/confirmemail?userId&token
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return NotFound();

            var result = await _accountRepository.ConfirmEmailAsync(userId, token);
            if (result.IsAuthenticated)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // api/account/addrole:
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (ModelState.IsValid)
            {
                model.Role = "User";
                var result = await _accountRepository.AddRoleAsync(model);

                if (!string.IsNullOrEmpty(result))
                {
                    return Ok(); // Status Code: 200 OK
                }
                return BadRequest(result); // Status Code: 400 BadRequest!!
            }
            return BadRequest(ModelState); // Status Code: 400 BadRequest!!
        }

        #endregion

        #region Helper

        // api/account/googleauthenticate
        /*[HttpPost("googleauthenticate")]
        public async Task<IActionResult> GoogleAuthenticate([FromBody] GoogleUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(it => it.Errors).Select(it => it.ErrorMessage));

            return Ok(await _accountRepository.AuthenticateGoogleUserAsync(request));
        }*/

        #endregion

    }
}

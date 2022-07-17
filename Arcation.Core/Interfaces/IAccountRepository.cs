using Arcation.Core.Models;
using Arcation.Core.ViewModels;
using Arcation.Core.ViewModels.ArcationViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces
{
    public interface IAccountRepository
    {
        Task<LoginResponseViewModel> RegisterAsync(RegisterViewModel model);
        Task<LoginResponseViewModel> LoginAsync(LoginViewModel model);
        Task<LoginResponseViewModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<LoginResponseViewModel> GenerateForgetPasswordTokenAsync(string email);
        Task<LoginResponseViewModel> ConfrimPasswordToken(ConfirmTokenFrogetPasswordViewModel model);
        Task<LoginResponseViewModel> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<LoginResponseViewModel> ConfirmEmailAsync(string userID, string token);
        Task<LoginResponseViewModel> AuthenticateGoogleUserAsync(GoogleUserRequest request);

        Task<LoginResponseViewModel> RegisterLeaderAsync(AddLeaderDto model);
        Task<AccountViewModel> GetUserDetail(string userId);
    }
}

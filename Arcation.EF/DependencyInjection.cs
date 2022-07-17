using Arcation.Core;
using Arcation.Core.Interfaces;
using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models;
using Arcation.EF.Helper;
using Arcation.EF.Repositories;
using Arcation.EF.Repositories.ArcationRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF
{
    public static class DependencyInjection
    {
        public static IServiceCollection ImplementPersistence(this IServiceCollection _services, IConfiguration _configuration)
        {
            // Configure EntityFramework with SqlServer : 
            _services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Configrue The Identity:
            _services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.User.AllowedUserNameCharacters = null;

                // Confirmation email required for new account
                options.SignIn.RequireConfirmedEmail = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;

            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<FourDigitTokenProvider>(FourDigitTokenProvider.FourDigitEmail)
            .AddTokenProvider<FourDigitTokenProvider>(FourDigitTokenProvider.FourDigitPhone);

            // JwtAuthentication : 
            _services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["Authentication:JWT:ValidAudience"],
                    ValidIssuer = _configuration["Authentication:JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:JWT:Secret"])),
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };

            });/*.AddGoogle(options =>
            {
                options.ClientId = _configuration["Authentication:Google:ClientId"];
                options.ClientSecret = _configuration["Authentication:Google:ClientSecret"];
            });*/


            _services.Configure<IdentityOptions>(options => 
            { 
            
            });


            _services.Configure<MailSettings>(_configuration.GetSection("MailSettings"));
            _services.Configure<AppURL>(_configuration.GetSection("AppURL"));
            _services.AddTransient<IMailingServices, MailingServices>();
            _services.AddTransient<IAccountRepository, AccountRepository>();
            _services.AddTransient<IUnitOfWork, UnitOfWork>();

            return _services;
        }
    }
}

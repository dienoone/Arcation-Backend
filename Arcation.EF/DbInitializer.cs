using Arcation.Core.Models;
using Arcation.Core.Models.ArcationModels.Main;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF
{
    public class DbInitializer
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeData()
        {
            ////////// Create Two Roles (Admin, User) and one admin account && one user account assigned with proper roles //////////

            var findAdminRole = await _roleManager.FindByNameAsync("Admin");
            var findUserRole = await _roleManager.FindByNameAsync("User");
            var adminRole = new IdentityRole("Admin");
            var userRole = new IdentityRole("User");

            //If admin role does not exists, create it
            if (findAdminRole == null)
            {
                await _roleManager.CreateAsync(adminRole);
            }
            //If user role does not exists, create it
            if (findUserRole == null)
            {
                await _roleManager.CreateAsync(userRole);
            }

            var findAdminAccount = await _userManager.FindByNameAsync("admin@gmail.com");

            //If there is no user account "admin@adps.com", create it       
            if (findAdminAccount == null)
            {
                var admin = new ApplicationUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "She Jong",
                    LastName = "Shon",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(admin, "P@$$w0rd");
                var account = await _userManager.FindByEmailAsync(admin.Email);
                account.EmailConfirmed = true;

                try
                {
                    if (result.Succeeded)
                    {
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    var e = ex;
                }
            }

            var adminAccount = await _userManager.FindByNameAsync("admin@gmail.com");
            //If Admin account is not in an admin role, add it to the role.
            if (!await _userManager.IsInRoleAsync(adminAccount, adminRole.Name))
            {
                await _userManager.AddToRoleAsync(adminAccount, adminRole.Name);
            }

            var findUserAccount = await _userManager.FindByNameAsync("user@gmail.com");
            //If there is no user account "test@gmail.com, create it       
            if (findUserAccount == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "user@gmail.com",
                    Email = "user@gmail.com",
                    FirstName = "Mike",
                    LastName = "Carson",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, "P@$$w0rd");
                var account = await _userManager.FindByEmailAsync(user.Email);
                account.EmailConfirmed = true;

                try
                {
                    if (result.Succeeded)
                    {
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    var e = ex;
                }
            }

            var userAccount = await _userManager.FindByNameAsync("user@gmail.com");
            //If User account is not in an User role, add it to the role.
            if (!await _userManager.IsInRoleAsync(userAccount, userRole.Name))
            {
                await _userManager.AddToRoleAsync(userAccount, userRole.Name);
            }

            var findUserAccount2 = await _userManager.FindByNameAsync("user2@gmail.com");
            //If there is no user account "test@gmail.com, create it       
            if (findUserAccount2 == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "user2@gmail.com",
                    Email = "user2@gmail.com",
                    FirstName = "Jennifer",
                    LastName = "Louse",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, "P@$$w0rd");
                var account = await _userManager.FindByEmailAsync(user.Email);
                account.EmailConfirmed = true;

                try
                {
                    if (result.Succeeded)
                    {
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    var e = ex;
                }
            }

            var userAccount2 = await _userManager.FindByNameAsync("user2@gmail.com");
            //If User account is not in an User role, add it to the role.
            if (!await _userManager.IsInRoleAsync(userAccount2, userRole.Name))
            {
                await _userManager.AddToRoleAsync(userAccount2, userRole.Name);
            }

            var findUserAccount3 = await _userManager.FindByNameAsync("user3@gmail.com");
            //If there is no user account "test@gmail.com, create it       
            if (findUserAccount3 == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "user3@gmail.com",
                    Email = "user3@gmail.com",
                    FirstName = "Peter",
                    LastName = "Hanson",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, "P@$$w0rd");
                var account = await _userManager.FindByEmailAsync(user.Email);
                account.EmailConfirmed = true;

                try
                {
                    if (result.Succeeded)
                    {
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    var e = ex;
                }
            }

            var userAccount3 = await _userManager.FindByNameAsync("user3@gmail.com");
            //If User account is not in an User role, add it to the role.
            if (!await _userManager.IsInRoleAsync(userAccount3, userRole.Name))
            {
                await _userManager.AddToRoleAsync(userAccount3, userRole.Name);
            }

            var companies = new Company[]
            {
                new Company { Name = "الشرق", Photo = "Photo1", BusinessId = adminAccount.Id, IsDeleted = false, CreatedBy = adminAccount.Id, CreatedAt = DateTime.Now},
                new Company { Name = "المقاولون العرب", Photo = "Photo2", BusinessId = adminAccount.Id, IsDeleted = false, CreatedBy = adminAccount.Id, CreatedAt = DateTime.Now},
                new Company { Name = "سعد زغلول", Photo = "Photo3", BusinessId = adminAccount.Id, IsDeleted = false, CreatedBy = adminAccount.Id, CreatedAt = DateTime.Now},
                new Company { Name = "النور", Photo = "Photo4", BusinessId = adminAccount.Id, IsDeleted = false, CreatedBy = adminAccount.Id, CreatedAt = DateTime.Now},
                new Company { Name = "الظلام", Photo = "Photo5", BusinessId = adminAccount.Id, IsDeleted = false, CreatedBy = adminAccount.Id, CreatedAt = DateTime.Now}
            };

            foreach(var company in companies)
            {
                _context.Companies.Add(company);
            }

            var bands = new Band[]
            {
                new Band {BandName = "دهانات", IsDeleted = false, CreatedAt = DateTime.Now, CreatedBy = adminAccount.Id, BusinessId = adminAccount.Id},
                new Band {BandName = "الكهرباء", IsDeleted = false, CreatedAt = DateTime.Now, CreatedBy = adminAccount.Id, BusinessId = adminAccount.Id},
                new Band {BandName = "السيراميك", IsDeleted = false, CreatedAt = DateTime.Now, CreatedBy = adminAccount.Id, BusinessId = adminAccount.Id},
                new Band {BandName = "البنا", IsDeleted = false, CreatedAt = DateTime.Now, CreatedBy = adminAccount.Id, BusinessId = adminAccount.Id},
                new Band {BandName = "المحاره", IsDeleted = false, CreatedAt = DateTime.Now, CreatedBy = adminAccount.Id, BusinessId = adminAccount.Id}
            };

            foreach(var band in bands)
            {
                _context.Bands.Add(band);
            }

            await _context.SaveChangesAsync();
        }
    }
}

using Arcation.Core.Configrations;
using Arcation.Core.Models;
using Arcation.Core.Models.ArcationModels.Main;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Tables : 

        #region MainTabel:

        public DbSet<Company> Companies { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<BandLocation> BandLocations { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<BLWested> BLWesteds { get; set; }

        #endregion

        #region Leaders :
        public DbSet<ExtractRow> ExtractRows { get; set; }
        public DbSet<Extract> Extracts { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Leader> Leaders { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Transiction> Transactions { get; set; }
        public DbSet<Wested> Westeds { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<BandLocationLeader> BandLocationLeaders { get; set; }
        public DbSet<BandLocationLeaderPeriod> BandLocationLeaderPeriods { get; set; }
        public DbSet<BandLocationLeaderPeriodEmployee> BandLocationLeaderPeriodEmployees { get; set; }
        public DbSet<BandLocationLeaderPeriodEmployeePeriod> BandLocationLeaderPeriodEmployeePeriods { get; set; }
        public DbSet<BandLocationLeaderPeriodEmployeePeriodAttendance> BandLocationLeaderPeriodEmployeePeriodAttendances { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            // Many To Many Relations:
            builder.ApplyConfigurationsFromAssembly(typeof(BandLocationEntityTypeConfigration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(BandLocationLeaderEntityTypeConfigration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(BandLocationLeaderPeriodEntityTypeConfigration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(BandLocationLeaderPeriodEmployeeEntityTypeConfigration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(BandLocationLeaderPeriodEmployeePeriodAttendanceEntityTypeConfigration).Assembly);

        }

    }
}

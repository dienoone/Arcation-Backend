using Arcation.Core.Models.ArcationModels.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Configrations
{
    public class BandLocationLeaderPeriodEmployeeEntityTypeConfigration : IEntityTypeConfiguration<BandLocationLeaderPeriodEmployee>
    {
        public void Configure(EntityTypeBuilder<BandLocationLeaderPeriodEmployee> builder)
        {
            builder
                .HasOne(b => b.Employee)
                .WithMany(bl => bl.BandLocationLeaderPeriodEmployees)
                .HasForeignKey(bi => bi.EmployeeId);


            builder
                .HasOne(l => l.BandLocationLeaderPeriod)
                .WithMany(bl => bl.BandLocationLeaderPeriodEmployees)
                .HasForeignKey(li => li.BandLocationLeaderPeriodId);

        }
    }
}

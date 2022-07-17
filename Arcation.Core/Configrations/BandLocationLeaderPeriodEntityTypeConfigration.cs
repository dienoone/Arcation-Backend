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
    public class BandLocationLeaderPeriodEntityTypeConfigration : IEntityTypeConfiguration<BandLocationLeaderPeriod>
    {
        public void Configure(EntityTypeBuilder<BandLocationLeaderPeriod> builder)
        {
            builder
                .HasOne(b => b.Period)
                .WithMany(bl => bl.BandLocationLeaderPeriods)
                .HasForeignKey(bi => bi.PeriodId);


            builder
                .HasOne(l => l.BandLocationLeader)
                .WithMany(bl => bl.BandLocationLeaderPeriods)
                .HasForeignKey(li => li.BandLocationLeaderId);

        }
    }
}

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
    public class BandLocationLeaderEntityTypeConfigration : IEntityTypeConfiguration<BandLocationLeader>
    {
        public void Configure(EntityTypeBuilder<BandLocationLeader> builder)
        {
            builder
                .HasOne(b => b.Leader)
                .WithMany(bl => bl.BandLocationLeaders)
                .HasForeignKey(bi => bi.LeaderId);


            builder
                .HasOne(l => l.BandLocation)
                .WithMany(bl => bl.BandLocationLeaders)
                .HasForeignKey(li => li.BandLocationId);

        }
    }
}

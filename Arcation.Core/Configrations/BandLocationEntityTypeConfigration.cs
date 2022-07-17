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
    public class BandLocationEntityTypeConfigration : IEntityTypeConfiguration<BandLocation>
    {
        public void Configure(EntityTypeBuilder<BandLocation> builder)
        {
            builder
                .HasOne(b => b.Band)
                .WithMany(bl => bl.BandLocations)
                .HasForeignKey(bi => bi.BandId);


            builder
                .HasOne(l => l.Location)
                .WithMany(bl => bl.BandLocations)
                .HasForeignKey(li => li.LocationId);

        }
    }
}

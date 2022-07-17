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
    public class BandLocationLeaderPeriodEmployeePeriodAttendanceEntityTypeConfigration : IEntityTypeConfiguration<BandLocationLeaderPeriodEmployeePeriodAttendance>
    {
        public void Configure(EntityTypeBuilder<BandLocationLeaderPeriodEmployeePeriodAttendance> builder)
        {
            builder
                .HasOne(b => b.Attendance)
                .WithMany(bl => bl.BandLocationLeaderPeriodEmployeePeriodAttendances)
                .HasForeignKey(bi => bi.AttendanceId);

            builder
                .HasOne(l => l.BandLocationLeaderPeriodEmployeePeriod)
                .WithMany(bl => bl.BandLocationLeaderPeriodEmployeePeriodAttendances)
                .HasForeignKey(li => li.BandLocationLeaderPeriodEmployeePeriodId);

        }
    }
}

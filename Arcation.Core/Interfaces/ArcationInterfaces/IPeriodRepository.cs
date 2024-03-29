﻿using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IPeriodRepository : IBaseRepository<Period>
    {
        Task<Period> GetPeriodDetail(int? periodId, string BusinessId);
        Task<Period> GetPeriodIncludeBandLocation(int? periodId, string businessId);
    }
}

using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IWestedRepository : IBaseRepository<Wested>
    {
        double GetPeriodReport(int? periodId, string businessId);
        double GetBandLocationInnerReport(int? bandLocationId, string businessId);
        double GetCompanyGlobalReport(int? companyId, string busiessId);
        double GetLocationGlobalReport(int? locationId, string busiessId);
        double GetBandGlobalReport(int? bandLocationId, string busiessId);
    }
}

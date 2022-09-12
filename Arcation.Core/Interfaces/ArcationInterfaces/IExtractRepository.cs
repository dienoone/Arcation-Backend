using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IExtractRepository : IBaseRepository<Extract>
    {
        Task<Extract> GetExtractAsync(int? bandLocationId, string businessId);
        Task<IEnumerable<Extract>> GetExtractsAsync(int? bandLocationId, string businessId);
        Task<IEnumerable<Extract>> SearchExtractAsync(int? bandLocationId, string name, string businessId);


        double GetBandLocationInnerReport(int? bandLocationId, string busiessId);
        double GetCompanyGlobalReport(int? companyId, string busiessId);
        double GetLocationGlobalReport(int? locationId, string busiessId);
        double GetBandGlobalReport(int? bandId, string busiessId);

    }
}

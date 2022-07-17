using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IBillRepository : IBaseRepository<Bill>
    {
        double GetTotalCompany(int? companyId);
        double GetTotalBand(int? bandId);
        double GetTotalLocation(int? locationId);
    }
}

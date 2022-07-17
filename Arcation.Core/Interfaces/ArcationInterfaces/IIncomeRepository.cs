using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IIncomeRepository : IBaseRepository<Income>
    {
        double GetTotalCompany(int? companyId);
        double GetTotalBand(int? bandId);
        double GetTotalLocation(int? locationId);
    }
}

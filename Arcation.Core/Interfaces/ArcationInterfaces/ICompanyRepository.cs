using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<IEnumerable<Company>> GlobalReport(string BusinessId);
        Task<Company> SingleGlobalReport(string BusinessId, int? Id);
    }
}

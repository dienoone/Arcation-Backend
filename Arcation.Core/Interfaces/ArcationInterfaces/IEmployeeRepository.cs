using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetAllIncludeTypes(string businessId);
        Task<IEnumerable<Employee>> GetSearch(string name, string businessId);
        Task<Employee> GetEmployeeIncludeTypes(int? Id, string businessId);
        Task<Employee> GetEmployeePeriods(int? Id, string businessId);
    }
}

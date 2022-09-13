using Arcation.Core.Models.ArcationModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface IBandLocationLeaderPeriodEmployeeRepository : IBaseRepository<BandLocationLeaderPeriodEmployee>
    {
        Task<List<BandLocationLeaderPeriodEmployee>> GetPeriodEmployeeDataAsync(int? bandLocationLeaderPeriodId, string bussinessId);
        Task<List<BandLocationLeaderPeriodEmployee>> SearchPeriodEmployeeDataAsync(int? bandLocationLeaderPeriodId, string name, string bussinessId);
        Task<BandLocationLeaderPeriodEmployee> GetSinglePeriodEmployeeDataAsync(int? bandLocationLeaderPeriodId, int? Id, string bussinessId);
        Task<IEnumerable<BandLocationLeaderPeriodEmployee>> GetForInitializeAttendance(int? bandLocationLeaderPeriodId, string bussinessId);

        Task<BandLocationLeaderPeriodEmployee> GetPeriodDetailsAsync(int? locationID, int? employeeID, string busniessId);
        Task<IEnumerable<BandLocationLeaderPeriodEmployee>> GetLocations(int? employeeId, string busniessId);
        Task<IEnumerable<EmployeeType>> GetEmployeeTypesCompany(int companyID);
        Task<IEnumerable<EmployeeType>> GetEmployeeTypesLocation(int locationID);
        Task<IEnumerable<EmployeeType>> GetEmployeeTypesBand(int bandId);

        int GetPeriodCountEmployees(int? periodId, string buseinssId);
        int GetBandLocationInnerReport(int? bandLocationId, int? typeId, string businessId);
        int GetNumberOfEmployeesInCompany(int? companyId, string businessId);
        int GetNumberOfEmployeesInLocation(int? locationId, string businessId);
        int GetNumberOfEmployeesInBand(int? bandId, string businessId);
        int GetNumberOfEmployeeTypeInCompany(int? companyId, int? typeId, string businessId);
        int GetNumberOfEmployeeTypeInLocation(int? locationId, int? typeId, string businessId);
        int GetNumberOfEmployeeTypeInBand(int? bandId, int? typeId, string businessId);
        Task<IEnumerable<BandLocationLeaderPeriodEmployee>> GetLeaderPeriodsAsync(int? periodId, string buinessId);
    }
}

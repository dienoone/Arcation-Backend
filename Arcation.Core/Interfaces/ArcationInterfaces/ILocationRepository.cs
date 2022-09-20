using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces.ArcationInterfaces
{
    public interface ILocationRepository : IBaseRepository<Location>
    {
        Task<Location> LocationAsync(int locationId);
        Task<List<Location>> GetAllLocationsAsync(string userID, int? companyId);
        Task<Location> GetLocation(string BusinessId, int? Id);
        Task<List<Location>> SearchLocationAsync(string userId, int? companyId, string name);
        Task<IEnumerable<Location>> LocationRelatedToEmployee(int? EmployeeId, string businessId);
        Task<IEnumerable<Location>> LocaionsWithBandsRelatedToLeader(string leaderId, string BusinessId);
        Task<Location> GetLocationReport(int? locationId, string businessId);
        Task<IEnumerable<Location>> SearchLocaionsWithBandsRelatedToLeader(string leaderId, string BusinessId, string name);
        int NumberOfLocationCompany(int? companyId, string busniessId);
    }
}

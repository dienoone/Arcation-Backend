﻿using Arcation.Core.Models.ArcationModels.Main;
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
        Task<List<Location>> GetAllLocationsAsync(string userID, int? companyId);
        Task<Location> GetLocationAsync(string userID, int? companyId, int? Id);
        Task<List<Location>> SearchLocationAsync(string userId, int? companyId, string name);
        Task<IEnumerable<Location>> LocationRelatedToEmployee(int? EmployeeId, string businessId);
        Task<IEnumerable<Location>> LocaionsWithBandsRelatedToLeader(string leaderId, string BusinessId);
        Task<Location> GetLocationReport(int? locationId, string businessId);
    }
}
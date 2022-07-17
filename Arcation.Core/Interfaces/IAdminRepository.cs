using Arcation.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces
{
    public interface IAdminRepository : IBaseRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetUsers();
        void EnableLockOut(string userName);
        void DisableLockOut(string userName);
    }
}

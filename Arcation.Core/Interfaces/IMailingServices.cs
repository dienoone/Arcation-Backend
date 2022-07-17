using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces
{
    public interface IMailingServices
    {
        Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null);
    }
}

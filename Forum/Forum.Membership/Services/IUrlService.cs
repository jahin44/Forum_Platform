using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Membership.Services
{
    public interface IUrlService
    {
        string GenerateAbsoluteUrl(string controller, string action, object parameters);
    }
}
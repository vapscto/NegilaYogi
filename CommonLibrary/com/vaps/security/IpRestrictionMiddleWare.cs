using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary.com.vaps.security
{
    public class IpRestrictionMiddleware
    {
        public readonly RequestDelegate Next;
        public readonly IpSecuritySettings IpSecuritySettings;

        public IpRestrictionMiddleware(RequestDelegate next, IOptions<IpSecuritySettings> ipSecuritySettings)
        {
            Next = next;
            IpSecuritySettings = ipSecuritySettings.Value;
        }

        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
        {
            var ipAddress = (string)context.Connection.RemoteIpAddress?.ToString();
            if (!IpSecuritySettings.AllowedIPsList.Contains(ipAddress))
            {
                context.Response.StatusCode = 403;
                return;
            }

            await Next(context);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary.com.vaps.security
{
    public class IpSecuritySettings
    {
        public string AllowedIPs { get; set; }
        public List<string> AllowedIPsList
        {
            get { return !string.IsNullOrEmpty(AllowedIPs) ? AllowedIPs.Split(',').ToList() : new List<string>(); }
        }
    }
}

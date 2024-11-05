using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;
using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;
using System.Net;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class changepwdFacadeController : Controller
    {
        public changepwdInterface _enq;

        public changepwdFacadeController(changepwdInterface enqui)
        {
            _enq = enqui;
        }

        // GET api/values/5
       
        [HttpPost]
        public async Task<regis> Post([FromBody] regis enquiry)
        {
            if (enquiry.changepasswordtypeflag == "FirstTimeLogin")
            {
                var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                string netip = remoteIpAddress.ToString();
                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                string myIP1 = ip_list.ToString();
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }

                enquiry.sMacAddress = sMacAddress;
                enquiry.netip = netip;
                enquiry.myIP1 = myIP1;
            }         

            return await _enq.saveEnqdata(enquiry);
        }       
    }
}
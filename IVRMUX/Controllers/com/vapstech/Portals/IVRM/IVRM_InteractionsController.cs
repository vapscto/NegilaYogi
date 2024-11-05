using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using System.Net.NetworkInformation;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class IVRM_InteractionsController : Controller
    {
        IVRM_InteractionsDelegate delgte = new IVRM_InteractionsDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public IVRM_School_InteractionsDTO getloaddata(int id)
        {
            IVRM_School_InteractionsDTO data = new IVRM_School_InteractionsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.getloaddata(data);
        }


        [Route("getdetails")]
        public IVRM_School_InteractionsDTO getstaff([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.getdetails(data);
        }

       
        [Route("getstudent")]
        public IVRM_School_InteractionsDTO getstudent([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delgte.getstudent(data);
        }
        [Route("savedetails")]
        public IVRM_School_InteractionsDTO savedetails([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();            String sMacAddress = string.Empty;            foreach (NetworkInterface adapter in nics)            {                if (sMacAddress == String.Empty)// only return MAC Address from first card
                {                    IPInterfaceProperties properties = adapter.GetIPProperties();                    sMacAddress = adapter.GetPhysicalAddress().ToString();                }            }            data.ISMINT_MACAddress = sMacAddress;            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;            data.ISMINT_ISPIPAddress = remoteIpAddress.ToString();
            return delgte.savedetails(data);
        }
     
        [Route("savereply")]
        public IVRM_School_InteractionsDTO savereply([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));


            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();            String sMacAddress = string.Empty;            foreach (NetworkInterface adapter in nics)            {                if (sMacAddress == String.Empty)// only return MAC Address from first card
                {                    IPInterfaceProperties properties = adapter.GetIPProperties();                    sMacAddress = adapter.GetPhysicalAddress().ToString();                }            }            data.ISMINT_MACAddress = sMacAddress;            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;            data.ISMINT_ISPIPAddress = remoteIpAddress.ToString();

            return delgte.savereply(data);
        }

        [Route("deletemsg")]
        public IVRM_School_InteractionsDTO deletemsg ([FromBody]IVRM_School_InteractionsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delgte.deletemsg(dto);
        }
         [Route("deleteinboxmsg")]
        public IVRM_School_InteractionsDTO deleteinboxmsg([FromBody]IVRM_School_InteractionsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delgte.deleteinboxmsg(dto);
        }


        [Route("reply")]
        public IVRM_School_InteractionsDTO reply([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return delgte.reply(data);
        }

        //delete 
        [Route("seen")]
        public IVRM_School_InteractionsDTO seen([FromBody]IVRM_School_InteractionsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return delgte.seen(data);
        }

    }
}

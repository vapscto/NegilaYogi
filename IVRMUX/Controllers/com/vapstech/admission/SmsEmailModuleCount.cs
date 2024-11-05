using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class SmsEmailModuleCount : Controller
    {
        SmsEmailModuleCountDelegate cwsd = new SmsEmailModuleCountDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public SmsEmailModuleCountDTO getalldetails(int id)
        {
            SmsEmailModuleCountDTO dto = new SmsEmailModuleCountDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cwsd.getdetails(dto);
        }
        [HttpPost]
        [Route("Getreportdetails")]
        public SmsEmailModuleCountDTO Getreportdetails([FromBody] SmsEmailModuleCountDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cwsd.Getreportdetails(data);
        }


    }
}

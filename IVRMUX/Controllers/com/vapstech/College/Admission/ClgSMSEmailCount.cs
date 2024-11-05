using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class ClgSMSEmailCount : Controller
    {
        ClgSMSEmailCountDelegate delobj = new ClgSMSEmailCountDelegate();


        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public ClgSMSEmailCountDTO Get123(int id)
        {
            ClgSMSEmailCountDTO data = new ClgSMSEmailCountDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        
            return delobj.getdata(data);
        }

        [HttpPost]
        [Route("getreport")]
        public ClgSMSEmailCountDTO getreport([FromBody] ClgSMSEmailCountDTO data123)
        {
            data123.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return delobj.getreport(data123);
        }
        [Route("SearchByColumn")]
        public ClgSMSEmailCountDTO SearchByColumn([FromBody] ClgSMSEmailCountDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.SearchByColumn(data);
        }
    }
}

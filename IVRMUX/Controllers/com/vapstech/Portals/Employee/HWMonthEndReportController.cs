using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Employee;
using Microsoft.AspNetCore.Http;

using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class HWMonthEndReportController : Controller
    {
        HWMonthEndReportDelegate hw = new HWMonthEndReportDelegate();



        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public HomeWorkUploadDTO Get123(int id)
        {
            HomeWorkUploadDTO data = new HomeWorkUploadDTO();
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mi_id;
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return hw.getdata123(data);
        }



        //  POST api/values

        [HttpPost]
        [Route("getreport")]
        public HomeWorkUploadDTO getreport([FromBody] HomeWorkUploadDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mi_id;
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return hw.getreport(data);
        }

       
    }
}

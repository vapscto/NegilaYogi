using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Hostel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Hostel
{
    [Route("api/[controller]")]
    public class StaffRequestController : Controller
    {

        StaffRequestDelegate del = new StaffRequestDelegate();

        [HttpPost]
        [Route("loaddata")]
        public StaffRequestDTO loaddata([FromBody] StaffRequestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return del.loaddata(data);

        }
        [Route("save")]
        public StaffRequestDTO save([FromBody]StaffRequestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return del.save(data);
        }
        [Route("edittab1")]
        public StaffRequestDTO edittab1([FromBody] StaffRequestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return del.edittab1(data);
        }
        [Route("deactive")]
        public StaffRequestDTO deactive([FromBody]StaffRequestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive(data);
        }


    }
}

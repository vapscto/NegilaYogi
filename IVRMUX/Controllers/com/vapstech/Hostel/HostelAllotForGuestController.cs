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
    public class HostelAllotForGuestController : Controller
    {
        HostelAllotForGuestDelegate del = new HostelAllotForGuestDelegate();
        [Route("loaddata/{id:int}")]
        public HostelAllotForGuest_DTO loaddata(int id)
        {
            HostelAllotForGuest_DTO data = new HostelAllotForGuest_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));           
            return del.loaddata(data);
        }
        [Route("save")]
        public HostelAllotForGuest_DTO save([FromBody] HostelAllotForGuest_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return del.save(data);
        }
        [Route("deactive")]
        public HostelAllotForGuest_DTO deactive([FromBody] HostelAllotForGuest_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive(data);
        }
        [Route("get_roomdetails")]
        public HostelAllotForGuest_DTO get_roomdetails([FromBody] HostelAllotForGuest_DTO data)
        {
           data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.get_roomdetails(data);
        }
        [Route("EditData")]
        public HostelAllotForGuest_DTO EditData([FromBody] HostelAllotForGuest_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.EditData(data);
        }
    }
}

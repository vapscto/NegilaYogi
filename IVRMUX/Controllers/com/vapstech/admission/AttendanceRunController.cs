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
    public class AttendanceRunController : Controller
    {
        AttendanceRunDelegate del = new AttendanceRunDelegate();
        [Route("loaddata/{id:int}")]
        public AttendanceRunDTO loaddata(int id)
        {
            AttendanceRunDTO data = new AttendanceRunDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return del.loaddata(data);
        }

        [Route("savedetails")]
        public AttendanceRunDTO savedetails([FromBody] AttendanceRunDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savedetails(data);
        }

        [Route("griddetails")]
        public AttendanceRunDTO griddetails([FromBody] AttendanceRunDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.griddetails(data);
        }

    }
}

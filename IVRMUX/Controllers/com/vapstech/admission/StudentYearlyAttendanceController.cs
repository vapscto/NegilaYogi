using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentYearlyAttendanceController : Controller
    {
        StudentYearlyAttendanceDelegate sad = new StudentYearlyAttendanceDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]

        [Route("getdetails")]
        public StudentAttendanceReportDTO getdetails()
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // var aa = sad.GetDataById(mi_id, "StudentAttendanceReportFacade/getinitialdata/");
            //CommonDTO cdto = (CommonDTO)aa;
            //return cdto;
            return sad.getinitialdata(mi_id);
        }

        // POST api/values
        [HttpPost]
        [Route("getAttendetails")]
        public StudentAttendanceReportDTO getAttendetails([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.getserdata(data);
        }


        [Route("getclass")]
        public StudentAttendanceReportDTO getclass([FromBody] StudentAttendanceReportDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.miid = mid;
            return sad.getclass(MMD);
        }

    }
}

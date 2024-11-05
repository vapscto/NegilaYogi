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
    public class StudenttcReportcustomController : Controller
    {
        StudenttcReportcustomDelegate studtc = new StudenttcReportcustomDelegate();
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
            return studtc.getinitialdata(mi_id);
        }

        [Route("getnameregno")]
        public StudentAttendanceReportDTO getstuddetails([FromBody]StudentAttendanceReportDTO value)
        {
            return studtc.getstuddetails(value);
        }


        [Route("getACS/{id:int}")]
        public StudentAttendanceReportDTO getacademicstudentlist(int id)
        {
            return studtc.getStudentlist(id);
        }
        // POST api/values
        [HttpPost]
        [Route("getTcdetails")]
        public StudentAttendanceReportDTO getTcdetails([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return studtc.getTCdata(data);
        }
        [Route("getTcdetailsbwmc")]
        public StudentAttendanceReportDTO getTcdetailsbwmc([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return studtc.getTcdetailsbwmc(data);
        }
        
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

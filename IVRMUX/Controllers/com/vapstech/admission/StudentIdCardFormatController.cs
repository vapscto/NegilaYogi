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
    public class StudentIdCardFormatController : Controller
    {
        StudentIdCardFormatDelegate _delg = new StudentIdCardFormatDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("OnLoadStudentIdCardDetails/{id:int}")]
        public StudentIdCardFormatDTO OnLoadStudentIdCardDetails(int id)
        {
            StudentIdCardFormatDTO data = new StudentIdCardFormatDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.OnLoadStudentIdCardDetails(data);
        }

        [Route("OnChangeYear")]
        public StudentIdCardFormatDTO OnChangeYear([FromBody] StudentIdCardFormatDTO data)
        {             
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));           
            return _delg.OnChangeYear(data);
        }

        [Route("OnChangeClass")]
        public StudentIdCardFormatDTO OnChangeClass([FromBody] StudentIdCardFormatDTO data)
        {             
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));           
            return _delg.OnChangeClass(data);
        }

        [Route("OnChangeSection")]
        public StudentIdCardFormatDTO OnChangeSection([FromBody] StudentIdCardFormatDTO data)
        {             
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));           
            return _delg.OnChangeSection(data);
        }

        [Route("GetReportDetails")]
        public StudentIdCardFormatDTO GetReportDetails([FromBody] StudentIdCardFormatDTO data)
        {             
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));           
            return _delg.GetReportDetails(data);
        }
    }
}

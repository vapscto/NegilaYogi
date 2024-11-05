using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Student
{
    [Route("api/[controller]")]
    public class StudentHallticketController : Controller
    {
        StudentHallticketDelegate _delg = new StudentHallticketDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetLoadData/{id:int}")]
        public StudentHallticketDTO GetLoadData(int id)
        {
            StudentHallticketDTO data = new StudentHallticketDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.GetLoadData(data);
        }

        [Route("GetExamDetails")]
        public StudentHallticketDTO GetExamDetails([FromBody] StudentHallticketDTO data)
        { 
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.GetExamDetails(data);
        }

        [Route("GetReport")]
        public StudentHallticketDTO GetReport([FromBody] StudentHallticketDTO data)
        { 
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.GetReport(data);
        }

    }
}

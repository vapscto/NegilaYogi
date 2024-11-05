using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.SeatingArrangment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.SeatingArrangment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.SeatingArrangment
{
    [Route("api/[controller]")]
    public class School_Absent_Student_EntryController : Controller
    {
        School_Absent_Student_EntryDelegate _delg = new School_Absent_Student_EntryDelegate();
           // GET: api/<controller>
           [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        } 

        [Route("GetAbsentStudentLoadData/{id:int}")]
        public School_Absent_Student_EntryDTO GetAbsentStudentLoadData(int id)
        {
            School_Absent_Student_EntryDTO data = new School_Absent_Student_EntryDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetAbsentStudentLoadData(data);
        }

        [Route("OnChangeYear")]
        public School_Absent_Student_EntryDTO OnChangeYear([FromBody] School_Absent_Student_EntryDTO data)
        {          
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));          
            return _delg.OnChangeYear(data);
        }

        [Route("OnChangeClass")]
        public School_Absent_Student_EntryDTO OnChangeClass([FromBody] School_Absent_Student_EntryDTO data)
        {          
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));          
            return _delg.OnChangeClass(data);
        }

        [Route("OnChangeSection")]
        public School_Absent_Student_EntryDTO OnChangeSection([FromBody] School_Absent_Student_EntryDTO data)
        {          
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));          
            return _delg.OnChangeSection(data);
        }

        [Route("SearchData")]
        public School_Absent_Student_EntryDTO SearchData([FromBody] School_Absent_Student_EntryDTO data)
        {          
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));          
            return _delg.SearchData(data);
        }

        [Route("SaveData")]
        public School_Absent_Student_EntryDTO SaveData([FromBody] School_Absent_Student_EntryDTO data)
        {          
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));          
            return _delg.SaveData(data);
        }


        // Absent Report
        [Route("GetAbsentStudentReportLoadData/{id:int}")]
        public School_Absent_Student_EntryDTO GetAbsentStudentReportLoadData(int id)
        {
            School_Absent_Student_EntryDTO data = new School_Absent_Student_EntryDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetAbsentStudentReportLoadData(data);
        }

        [Route("OnChangeYearAbsentReport")]
        public School_Absent_Student_EntryDTO OnChangeYearAbsentReport([FromBody] School_Absent_Student_EntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeYearAbsentReport(data);
        }

        [Route("OnChangeClassAbsentReport")]
        public School_Absent_Student_EntryDTO OnChangeClassAbsentReport([FromBody] School_Absent_Student_EntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeClassAbsentReport(data);
        }

        [Route("OnChangeSectionAbsentReport")]
        public School_Absent_Student_EntryDTO OnChangeSectionAbsentReport([FromBody] School_Absent_Student_EntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeSectionAbsentReport(data);
        }

        [Route("GetSchoolAbsentStudentReport")]
        public School_Absent_Student_EntryDTO GetSchoolAbsentStudentReport([FromBody] School_Absent_Student_EntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetSchoolAbsentStudentReport(data);
        }
    }
}

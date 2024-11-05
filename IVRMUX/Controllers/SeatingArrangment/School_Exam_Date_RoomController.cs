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
    public class School_Exam_Date_RoomController : Controller
    {
        School_Exam_Date_RoomDelegate _delg = new School_Exam_Date_RoomDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetExamDateRoomloaddata/{id:int}")]
        public School_Exam_Date_RoomDTO GetExamDateRoomloaddata(int id)
        {
            School_Exam_Date_RoomDTO data = new School_Exam_Date_RoomDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetExamDateRoomloaddata(data);
        }

        [Route("GetSearchExamDateRoomData")]
        public School_Exam_Date_RoomDTO GetSearchExamDateRoomData([FromBody] School_Exam_Date_RoomDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetSearchExamDateRoomData(data);
        }

        [Route("SaveExamDateRoomData")]
        public School_Exam_Date_RoomDTO SaveExamDateRoomData([FromBody] School_Exam_Date_RoomDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveExamDateRoomData(data);
        }

        [Route("EditExamDateRoomData")]
        public School_Exam_Date_RoomDTO EditExamDateRoomData([FromBody] School_Exam_Date_RoomDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditExamDateRoomData(data);
        }

        [Route("ViewExamDateRoomDetails")]
        public School_Exam_Date_RoomDTO ViewExamDateRoomDetails([FromBody] School_Exam_Date_RoomDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewExamDateRoomDetails(data);
        }

        [Route("ActiveDeactiveExamRoomDate")]
        public School_Exam_Date_RoomDTO ActiveDeactiveExamRoomDate([FromBody] School_Exam_Date_RoomDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.ActiveDeactiveExamRoomDate(data);
        }

        [Route("ActiveDeactiveExamDateRoomDetails")]
        public School_Exam_Date_RoomDTO ActiveDeactiveExamDateRoomDetails([FromBody] School_Exam_Date_RoomDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.ActiveDeactiveExamDateRoomDetails(data);
        }


        // Room Dates Class Subject Mapping
        [Route("GetExamDateRoomClassMappingloaddata/{id:int}")]
        public School_Exam_Date_RoomDTO GetExamDateRoomClassMappingloaddata(int id)
        {
            School_Exam_Date_RoomDTO data = new School_Exam_Date_RoomDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetExamDateRoomClassMappingloaddata(data);
        }

        [Route("GetSubjectListRoomClassMapping")]
        public School_Exam_Date_RoomDTO GetSubjectListRoomClassMapping([FromBody] School_Exam_Date_RoomDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return _delg.GetSubjectListRoomClassMapping(data);
        }

        [Route("GetSearchExamDateRoomClassMappingData")]
        public School_Exam_Date_RoomDTO GetSearchExamDateRoomClassMappingData([FromBody] School_Exam_Date_RoomDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return _delg.GetSearchExamDateRoomClassMappingData(data);
        }

        [Route("SaveExamDateRoomClassMappingData")]
        public School_Exam_Date_RoomDTO SaveExamDateRoomClassMappingData([FromBody] School_Exam_Date_RoomDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return _delg.SaveExamDateRoomClassMappingData(data);
        }

        [Route("EditExamDateRoomClassMappingData")]
        public School_Exam_Date_RoomDTO EditExamDateRoomClassMappingData([FromBody] School_Exam_Date_RoomDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return _delg.EditExamDateRoomClassMappingData(data);
        }

        [Route("ViewExamDateRoomClassMappingDetails")]
        public School_Exam_Date_RoomDTO ViewExamDateRoomClassMappingDetails([FromBody] School_Exam_Date_RoomDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return _delg.ViewExamDateRoomClassMappingDetails(data);
        }       

        [Route("ActiveDeactiveExamRoomClassMappingDate")]
        public School_Exam_Date_RoomDTO ActiveDeactiveExamRoomClassMappingDate([FromBody] School_Exam_Date_RoomDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return _delg.ActiveDeactiveExamRoomClassMappingDate(data);
        }     

        [Route("ActiveDeactiveExamDateRoomClassMappingDetails")]
        public School_Exam_Date_RoomDTO ActiveDeactiveExamDateRoomClassMappingDetails([FromBody] School_Exam_Date_RoomDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return _delg.ActiveDeactiveExamDateRoomClassMappingDetails(data);
        }


        // Seating Arrangement Allotment
        [Route("SchoolSAAllotmentloaddata/{id:int}")]
        public School_Exam_Date_RoomDTO SchoolSAAllotmentloaddata(int id)
        {
            School_Exam_Date_RoomDTO data = new School_Exam_Date_RoomDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.SchoolSAAllotmentloaddata(data);
        }

        [Route("SchoolGenerateSeatAllotment")]
        public School_Exam_Date_RoomDTO SchoolGenerateSeatAllotment([FromBody] School_Exam_Date_RoomDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SchoolGenerateSeatAllotment(data);
        }

        // Seat Allotment Report
        [Route("GetSeatAllotedReport/{id:int}")]
        public School_Exam_Date_RoomDTO GetSeatAllotedReport(int id)
        {
            School_Exam_Date_RoomDTO data = new School_Exam_Date_RoomDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetSeatAllotedReport(data);
        }

        [Route("GetSchoolSeatAllotementReport")]
        public School_Exam_Date_RoomDTO GetSchoolSeatAllotementReport([FromBody] School_Exam_Date_RoomDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetSchoolSeatAllotementReport(data);
        }

    }
}

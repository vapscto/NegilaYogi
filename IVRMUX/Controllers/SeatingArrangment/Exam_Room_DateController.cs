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
    public class Exam_Room_DateController : Controller
    {
        Exam_Room_DateDelegate _delg = new Exam_Room_DateDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
                
        /* Room Transaction Exam Date With Student Allotment */

        [Route("GetExamDateloaddata/{id:int}")]
        public Exam_Room_DateDTO GetExamDateloaddata(int id)
        {
            Exam_Room_DateDTO data = new Exam_Room_DateDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetExamDateloaddata(data);
        }

        [Route("GetSearchExamDateData")]
        public Exam_Room_DateDTO GetSearchExamDateData([FromBody] Exam_Room_DateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetSearchExamDateData(data);
        }

        [Route("SaveExamDateData")]
        public Exam_Room_DateDTO SaveExamDateData([FromBody] Exam_Room_DateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveExamDateData(data);
        }

        [Route("EditExamDateData")]
        public Exam_Room_DateDTO EditExamDateData([FromBody] Exam_Room_DateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditExamDateData(data);
        }

        [Route("ViewRoomDetails")]
        public Exam_Room_DateDTO ViewRoomDetails([FromBody] Exam_Room_DateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewRoomDetails(data);
        }

        [Route("ActiveDeactiveExamDate")]
        public Exam_Room_DateDTO ActiveDeactiveExamDate([FromBody] Exam_Room_DateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ActiveDeactiveExamDate(data);
        }

        [Route("ActiveDeactiveRoomDetails")]
        public Exam_Room_DateDTO ActiveDeactiveRoomDetails([FromBody] Exam_Room_DateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ActiveDeactiveRoomDetails(data);
        }

        [Route("CheckCount")]
        public Exam_Room_DateDTO CheckCount([FromBody] Exam_Room_DateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.CheckCount(data);
        }

        /* Room Sitting Style Details */
        [Route("GetRoomSittingStyleloaddata/{id:int}")]
        public Exam_Room_DateDTO GetRoomSittingStyleloaddata(int id)
        {
            Exam_Room_DateDTO data = new Exam_Room_DateDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetRoomSittingStyleloaddata(data);
        }

        [Route("SaveRoomSittingStyle")]
        public Exam_Room_DateDTO SaveRoomSittingStyle([FromBody] Exam_Room_DateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveRoomSittingStyle(data);
        }
    }
}

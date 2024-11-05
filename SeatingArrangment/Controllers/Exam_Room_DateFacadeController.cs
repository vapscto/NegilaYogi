using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.SeatingArrangment;
using SeatingArrangment.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeatingArrangment.Controllers
{
    [Route("api/[controller]")]
    public class Exam_Room_DateFacadeController : Controller
    {
        public Exam_Room_DateInterface _interface;
        public Exam_Room_DateFacadeController(Exam_Room_DateInterface _inter)
        {
            _interface = _inter;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /* Room Transaction Exam Date With Student Allotment */
        [Route("GetExamDateloaddata")]
        public Exam_Room_DateDTO GetExamDateloaddata([FromBody] Exam_Room_DateDTO data)
        { 
            return _interface.GetExamDateloaddata(data);
        }

        [Route("GetSearchExamDateData")]
        public Exam_Room_DateDTO GetSearchExamDateData([FromBody] Exam_Room_DateDTO data)
        {
            return _interface.GetSearchExamDateData(data);
        }

        [Route("SaveExamDateData")]
        public Exam_Room_DateDTO SaveExamDateData([FromBody] Exam_Room_DateDTO data)
        {
            return _interface.SaveExamDateData(data);
        }

        [Route("EditExamDateData")]
        public Exam_Room_DateDTO EditExamDateData([FromBody] Exam_Room_DateDTO data)
        {
            return _interface.EditExamDateData(data);
        }

        [Route("ViewRoomDetails")]
        public Exam_Room_DateDTO ViewRoomDetails([FromBody] Exam_Room_DateDTO data)
        {
            return _interface.ViewRoomDetails(data);
        }

        [Route("ActiveDeactiveExamDate")]
        public Exam_Room_DateDTO ActiveDeactiveExamDate([FromBody] Exam_Room_DateDTO data)
        {
            return _interface.ActiveDeactiveExamDate(data);
        }

        [Route("ActiveDeactiveRoomDetails")]
        public Exam_Room_DateDTO ActiveDeactiveRoomDetails([FromBody] Exam_Room_DateDTO data)
        {
            return _interface.ActiveDeactiveRoomDetails(data);
        }

        [Route("CheckCount")]
        public Exam_Room_DateDTO CheckCount([FromBody] Exam_Room_DateDTO data)
        {
            return _interface.CheckCount(data);
        }

        /* Room Sitting Style Details */
        [Route("GetRoomSittingStyleloaddata")]
        public Exam_Room_DateDTO GetRoomSittingStyleloaddata([FromBody] Exam_Room_DateDTO data)
        {
            return _interface.GetRoomSittingStyleloaddata(data);
        }

        [Route("SaveRoomSittingStyle")]
        public Exam_Room_DateDTO SaveRoomSittingStyle([FromBody] Exam_Room_DateDTO data)
        {
            return _interface.SaveRoomSittingStyle(data);
        }
    }
}

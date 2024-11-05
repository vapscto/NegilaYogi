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
    public class School_Exam_Date_RoomFacadeController : Controller
    {
        School_Exam_Date_RoomInterface _interface;
      
        public School_Exam_Date_RoomFacadeController(School_Exam_Date_RoomInterface _inter)
        {
            _interface = _inter;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetExamDateRoomloaddata")]
        public School_Exam_Date_RoomDTO GetExamDateRoomloaddata([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.GetExamDateRoomloaddata(data);
        }

        [Route("GetSearchExamDateRoomData")]
        public School_Exam_Date_RoomDTO GetSearchExamDateRoomData([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.GetSearchExamDateRoomData(data);
        }

        [Route("SaveExamDateRoomData")]
        public School_Exam_Date_RoomDTO SaveExamDateRoomData([FromBody] School_Exam_Date_RoomDTO data)
        {
           
            return _interface.SaveExamDateRoomData(data);
        }

        [Route("EditExamDateRoomData")]
        public School_Exam_Date_RoomDTO EditExamDateRoomData([FromBody] School_Exam_Date_RoomDTO data)
        {
          
            return _interface.EditExamDateRoomData(data);
        }

        [Route("ViewExamDateRoomDetails")]
        public School_Exam_Date_RoomDTO ViewExamDateRoomDetails([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.ViewExamDateRoomDetails(data);
        }

        [Route("ActiveDeactiveExamRoomDate")]
        public School_Exam_Date_RoomDTO ActiveDeactiveExamRoomDate([FromBody] School_Exam_Date_RoomDTO data)
        {
           
            return _interface.ActiveDeactiveExamRoomDate(data);
        }

        [Route("ActiveDeactiveExamDateRoomDetails")]
        public School_Exam_Date_RoomDTO ActiveDeactiveExamDateRoomDetails([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.ActiveDeactiveExamDateRoomDetails(data);
        }


        // Room Date Class Subject Mapping 
        [Route("GetExamDateRoomClassMappingloaddata")]
        public School_Exam_Date_RoomDTO GetExamDateRoomClassMappingloaddata([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.GetExamDateRoomClassMappingloaddata(data);
        }

        [Route("GetSubjectListRoomClassMapping")]
        public School_Exam_Date_RoomDTO GetSubjectListRoomClassMapping([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.GetSubjectListRoomClassMapping(data);
        }

        [Route("GetSearchExamDateRoomClassMappingData")]
        public School_Exam_Date_RoomDTO GetSearchExamDateRoomClassMappingData([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.GetSearchExamDateRoomClassMappingData(data);
        }

        [Route("SaveExamDateRoomClassMappingData")]
        public School_Exam_Date_RoomDTO SaveExamDateRoomClassMappingData([FromBody] School_Exam_Date_RoomDTO data)
        {

            return _interface.SaveExamDateRoomClassMappingData(data);
        }

        [Route("EditExamDateRoomClassMappingData")]
        public School_Exam_Date_RoomDTO EditExamDateRoomClassMappingData([FromBody] School_Exam_Date_RoomDTO data)
        {

            return _interface.EditExamDateRoomClassMappingData(data);
        }

        [Route("ViewExamDateRoomClassMappingDetails")]
        public School_Exam_Date_RoomDTO ViewExamDateRoomClassMappingDetails([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.ViewExamDateRoomClassMappingDetails(data);
        }

        [Route("ActiveDeactiveExamRoomClassMappingDate")]
        public School_Exam_Date_RoomDTO ActiveDeactiveExamRoomClassMappingDate([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.ActiveDeactiveExamRoomClassMappingDate(data);
        }

        [Route("ActiveDeactiveExamDateRoomClassMappingDetails")]
        public School_Exam_Date_RoomDTO ActiveDeactiveExamDateRoomClassMappingDetails([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.ActiveDeactiveExamDateRoomClassMappingDetails(data);
        }

        // Seating Arrangment Allotment
        [Route("SchoolSAAllotmentloaddata")]
        public School_Exam_Date_RoomDTO SchoolSAAllotmentloaddata([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.SchoolSAAllotmentloaddata(data);
        }

        [Route("SchoolGenerateSeatAllotment")]
        public School_Exam_Date_RoomDTO SchoolGenerateSeatAllotment([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.SchoolGenerateSeatAllotment(data);
        }

        // Seat Allotment Report

        [Route("GetSeatAllotedReport")]
        public School_Exam_Date_RoomDTO GetSeatAllotedReport([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.GetSeatAllotedReport(data);
        }

        [Route("GetSchoolSeatAllotementReport")]
        public School_Exam_Date_RoomDTO GetSchoolSeatAllotementReport([FromBody] School_Exam_Date_RoomDTO data)
        {
            return _interface.GetSchoolSeatAllotementReport(data);
        }

    }
}

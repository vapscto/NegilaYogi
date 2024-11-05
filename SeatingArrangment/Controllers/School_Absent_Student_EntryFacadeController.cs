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
    public class School_Absent_Student_EntryFacadeController : Controller
    {
        public School_Absent_Student_EntryInterface _interface;

        public School_Absent_Student_EntryFacadeController(School_Absent_Student_EntryInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetAbsentStudentLoadData")]
        public School_Absent_Student_EntryDTO GetAbsentStudentLoadData([FromBody] School_Absent_Student_EntryDTO data)
        { 
            return _interface.GetAbsentStudentLoadData(data);
        }

        [Route("OnChangeYear")]
        public School_Absent_Student_EntryDTO OnChangeYear([FromBody] School_Absent_Student_EntryDTO data)
        {
            return _interface.OnChangeYear(data);
        }

        [Route("OnChangeClass")]
        public School_Absent_Student_EntryDTO OnChangeClass([FromBody] School_Absent_Student_EntryDTO data)
        {             
            return _interface.OnChangeClass(data);
        }

        [Route("OnChangeSection")]
        public School_Absent_Student_EntryDTO OnChangeSection([FromBody] School_Absent_Student_EntryDTO data)
        {
            return _interface.OnChangeSection(data);
        }

        [Route("SearchData")]
        public School_Absent_Student_EntryDTO SearchData([FromBody] School_Absent_Student_EntryDTO data)
        {
            return _interface.SearchData(data);
        }

        [Route("SaveData")]
        public School_Absent_Student_EntryDTO SaveData([FromBody] School_Absent_Student_EntryDTO data)
        {
            return _interface.SaveData(data);
        }


        // Absent Report
        [Route("GetAbsentStudentReportLoadData")]
        public School_Absent_Student_EntryDTO GetAbsentStudentReportLoadData([FromBody] School_Absent_Student_EntryDTO data)
        {
            return _interface.GetAbsentStudentReportLoadData(data);
        }

        [Route("OnChangeYearAbsentReport")]
        public School_Absent_Student_EntryDTO OnChangeYearAbsentReport([FromBody] School_Absent_Student_EntryDTO data)
        {
            return _interface.OnChangeYearAbsentReport(data);
        }

        [Route("OnChangeClassAbsentReport")]
        public School_Absent_Student_EntryDTO OnChangeClassAbsentReport([FromBody] School_Absent_Student_EntryDTO data)
        {
            return _interface.OnChangeClassAbsentReport(data);
        }

        [Route("OnChangeSectionAbsentReport")]
        public School_Absent_Student_EntryDTO OnChangeSectionAbsentReport([FromBody] School_Absent_Student_EntryDTO data)
        {
            return _interface.OnChangeSectionAbsentReport(data);
        }

        [Route("GetSchoolAbsentStudentReport")]
        public School_Absent_Student_EntryDTO GetSchoolAbsentStudentReport([FromBody] School_Absent_Student_EntryDTO data)
        {
            return _interface.GetSchoolAbsentStudentReport(data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentIdCardFormatFacadeController : Controller
    {
        StudentIdCardFormatInterface _interface;

        public StudentIdCardFormatFacadeController (StudentIdCardFormatInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("OnLoadStudentIdCardDetails")]
        public StudentIdCardFormatDTO OnLoadStudentIdCardDetails([FromBody] StudentIdCardFormatDTO data)
        { 
            return _interface.OnLoadStudentIdCardDetails(data);
        }

        [Route("OnChangeYear")]
        public StudentIdCardFormatDTO OnChangeYear([FromBody] StudentIdCardFormatDTO data)
        {
            return _interface.OnChangeYear(data);
        }

        [Route("OnChangeClass")]
        public StudentIdCardFormatDTO OnChangeClass([FromBody] StudentIdCardFormatDTO data)
        {
            return _interface.OnChangeClass(data);
        }

        [Route("OnChangeSection")]
        public StudentIdCardFormatDTO OnChangeSection([FromBody] StudentIdCardFormatDTO data)
        { 
            return _interface.OnChangeSection(data);
        }

        [Route("GetReportDetails")]
        public StudentIdCardFormatDTO GetReportDetails([FromBody] StudentIdCardFormatDTO data)
        { 
            return _interface.GetReportDetails(data);
        }
    }
}

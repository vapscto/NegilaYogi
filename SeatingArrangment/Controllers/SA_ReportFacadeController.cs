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
    public class SA_ReportFacadeController : Controller
    {
        public SA_ReportInterface _interface;
        public SA_ReportFacadeController(SA_ReportInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetExamDateloaddata")]
        public SA_ReportDTO GetExamDateloaddata ([FromBody] SA_ReportDTO data)
        {
            return _interface.GetExamDateloaddata(data);
        }

        [Route("OnChangeyear")]
        public SA_ReportDTO OnChangeyear([FromBody] SA_ReportDTO data)
        {
            return _interface.OnChangeyear(data);
        }

        [Route("GetAbsentStudentReport")]
        public SA_ReportDTO GetAbsentStudentReport([FromBody] SA_ReportDTO data)
        {
            return _interface.GetAbsentStudentReport(data);
        }

        [Route("GetMalpracticeStudentReport")]
        public SA_ReportDTO GetMalpracticeStudentReport([FromBody] SA_ReportDTO data)
        {
            return _interface.GetMalpracticeStudentReport(data);
        }

    }
}

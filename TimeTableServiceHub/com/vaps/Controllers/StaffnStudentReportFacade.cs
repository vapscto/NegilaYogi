using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTableServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StaffnStudentReportFacade : Controller
    {
        public StaffnStudentReportInterface _ttbreaktime;
        public StaffnStudentReportFacade(StaffnStudentReportInterface maspag)
        {
            _ttbreaktime = maspag;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       
        [Route("getdetails/{id:int}")]
        public TT_StaffnStudentReportDTO getorgdet(int id)
        {
            return _ttbreaktime.getdetails(id);
        }
        [Route("getrpt")]
        public TT_StaffnStudentReportDTO Post([FromBody] TT_StaffnStudentReportDTO org)
        {
            return _ttbreaktime.getreport(org);
        }


    }
}

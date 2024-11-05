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
    public class CLGTTSubjectWiseReportFacadeController : Controller
    {
        public CLGTTSubjectWiseReportInterface _ttbreaktime;
        public CLGTTSubjectWiseReportFacadeController(CLGTTSubjectWiseReportInterface maspag)
        {
            _ttbreaktime = maspag;
        }
      
      [Route("getdetails")]
      public CLGTTSubjectWiseReportDTO getdetails([FromBody] CLGTTSubjectWiseReportDTO data)
        {
            return _ttbreaktime.getdetails(data);
        }
        [Route("getbranch")]

        public CLGTTSubjectWiseReportDTO getbranch([FromBody] CLGTTSubjectWiseReportDTO data)
        {
            return _ttbreaktime.getbranch(data);
        }
        [Route("getsemister")]
        public CLGTTSubjectWiseReportDTO getsemister([FromBody] CLGTTSubjectWiseReportDTO data)
        {
            return _ttbreaktime.getsemister(data);
        }
        [Route("savedetail")]
        public CLGTTSubjectWiseReportDTO savedetail([FromBody] CLGTTSubjectWiseReportDTO data)
        {
            return _ttbreaktime.savedetail(data);
        }


    }
}

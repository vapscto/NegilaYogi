using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegePreadmission.Interfaces;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Preadmission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePreadmission.Controllers
{
    [Route("api/[controller]")]
    public class PreLiveMeetingScheduleClgFacade : Controller
    {
        public PreLiveMeetingScheduleClgInterface _org;
        private readonly DomainModelMsSqlServerContext _db;
       private readonly ILogger<PreLiveMeetingScheduleClgInterface> _log;
        // GET: api/<controller>
        public PreLiveMeetingScheduleClgFacade(PreLiveMeetingScheduleClgInterface org, DomainModelMsSqlServerContext db, ILogger<PreLiveMeetingScheduleClgInterface> loggerFactor)
        {
            _org = org;
            _db = db;
            _log = loggerFactor;
        }

        [Route("getempdetails")]
        public PreLiveMeetingScheduleClgDTO getempdetails([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            return _org.getempdetails(data);
        }

        [Route("ondatechangestudent")]
        public PreLiveMeetingScheduleClgDTO ondatechangestudent([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            return _org.ondatechangestudent(data);
        }

        [Route("onschedulecahnge")]
        public PreLiveMeetingScheduleClgDTO onschedulecahnge([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            return _org.onschedulecahnge(data);
        }

        [Route("endmainmeetingstudent")]
        public PreLiveMeetingScheduleClgDTO endmainmeetingstudent([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            return _org.endmainmeetingstudent(data);
        }

        [Route("onstartmeeting")]
        public PreLiveMeetingScheduleClgDTO onstartmeeting([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            return _org.onstartmeeting(data);
        }
        [Route("saveremarks")]
        public PreLiveMeetingScheduleClgDTO saveremarks([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            return _org.saveremarks(data);
        }

        [Route("getstudentdetails")]
        public PreLiveMeetingScheduleClgDTO getstudentdetails([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            return _org.getstudentdetails(data);
        }

        [Route("joinmeetingStudent")]
        public PreLiveMeetingScheduleClgDTO joinmeetingStudent([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            return _org.joinmeetingStudent(data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using MobileApp.Delegates;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MobileApp.Controllers
{
    [Route("api/[controller]")]
    public class SmartcardattendanceController : Controller
    {
        public ILogger<class_section_list> _log;
        SmartcardattendanceDelegate smart = new SmartcardattendanceDelegate();
        public SmartcardattendanceController(ILogger<class_section_list> log)
        {
            _log = log;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        //getting onload data
        [Route("getSmartCardData")]
        public class_section_list getSmartCardData([FromBody]class_section_list data)
        {
            return smart.getSmartCardData(data);
        }

        //saving punched attendance details
        [Route("SaveSmartCardData")]
        public class_section_list SaveSmartCardData([FromBody]class_section_list data)
        {
            _log.LogInformation("Mobile App Error Smart card attendance  save data list  : " + data);
            return smart.SaveSmartCardData(data);
        }

        //getting student list who are not punched 
        [Route("getstudentdetailssmart")]
        public class_section_list getstudentdetailssmart([FromBody]class_section_list data)
        {
            _log.LogInformation("Mobile App Error Smart card attendance  getstudentdetailssmart data list  : " + data);
            return smart.getstudentdetailssmart(data);
        }


        //transfering smart card attendance to main attendance 
        [Route("saveattendancesmartcard")]
        public StudentAttendanceEntryDTO saveattendancesmartcard([FromBody]StudentAttendanceEntryDTO data)
        {
            _log.LogInformation("Mobile App Error Smart card attendance saveattendancesmartcard data list : " + data);
            return smart.saveattendancesmartcard(data);
        }

        //sending sms auto triggering 
        [Route("sendsmsabsent")]
        public StudentAttendanceEntryDTO sendsmsabsent([FromBody]StudentAttendanceEntryDTO data)
        {
            return smart.sendsmsabsent(data);
        }

        // College Smartcard attendance

        [Route("getalldetails")]
        public CollegeMultiHoursAttendanceEntryDTO getalldetails([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            return smart.getalldetails(data);
        }
        [Route("getBranchdata")]
        public CollegeMultiHoursAttendanceEntryDTO getBranchdata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            return smart.getBranchdata(data);
        }
        [Route("getSemesterdata")]
        public CollegeMultiHoursAttendanceEntryDTO getSemesterdata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            return smart.getSemesterdata(data);
        }
        [Route("getSectiondata")]
        public CollegeMultiHoursAttendanceEntryDTO getSectiondata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            return smart.getSectiondata(data);
        }
        [Route("getSubjdata")]
        public CollegeMultiHoursAttendanceEntryDTO getSubjdata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            return smart.getSubjdata(data);
        }
        [Route("getBatchdata")]
        public CollegeMultiHoursAttendanceEntryDTO getBatchdata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            return smart.getBatchdata(data);
        }


        
            

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class OveralldailyattendanceabsentsmsFacadeController : Controller
    {
        public overalldailyattendanceabsentsmsInterface _AttenRpt;

        public OveralldailyattendanceabsentsmsFacadeController(overalldailyattendanceabsentsmsInterface AttenRpt)
        {
            _AttenRpt = AttenRpt;
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
        [Route("getinitialdata")]
        public Task<OveralldailyattendanceabsentsmsDTO> getinitialdata([FromBody]OveralldailyattendanceabsentsmsDTO mi_id)
        {
            return _AttenRpt.getInitailData(mi_id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("getserdata")]
        public Task<OveralldailyattendanceabsentsmsDTO> getserdata([FromBody] OveralldailyattendanceabsentsmsDTO data)
        {
            return _AttenRpt.getserdata(data);
        }

        [Route("getstudentDet")]
        public OveralldailyattendanceabsentsmsDTO getStudentDet([FromBody] OveralldailyattendanceabsentsmsDTO dto)
        {
            return _AttenRpt.getstudentDet(dto);
        }
        [Route("sendsms")]
        public OveralldailyattendanceabsentsmsDTO sendsms([FromBody] OveralldailyattendanceabsentsmsDTO dto)
        {
            return _AttenRpt.sendsms(dto);
        }
        [Route("sendsms_twice")]
        public OveralldailyattendanceabsentsmsDTO sendsms_twice([FromBody] OveralldailyattendanceabsentsmsDTO dto)
        {
            return _AttenRpt.sendsms_twice(dto);
        }

        
        [Route("sendemail")]
        public OveralldailyattendanceabsentsmsDTO sendemail([FromBody] OveralldailyattendanceabsentsmsDTO dto)
        {
            return _AttenRpt.sendemail(dto);
        }
        [Route("smartcardatt")]
        public OveralldailyattendanceabsentsmsDTO smartcardatt([FromBody] OveralldailyattendanceabsentsmsDTO dto)
        {
            return _AttenRpt.smartcardatt(dto);
        }
        [Route("createuser")]
        public OveralldailyattendanceabsentsmsDTO createuser([FromBody] OveralldailyattendanceabsentsmsDTO dto)
        {
            return _AttenRpt.createuser(dto);
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

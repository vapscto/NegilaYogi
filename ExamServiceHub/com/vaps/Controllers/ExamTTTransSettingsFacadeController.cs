using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamTTTransSettingsFacadeController : Controller
    {
        private ExamTTTransSettingsInterface _inter;
        public ExamTTTransSettingsFacadeController(ExamTTTransSettingsInterface obj)
        {
            _inter = obj;
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
        [Route("deactivate")]
        public ExamTTTransSettingsDTO deactivateAcdmYear([FromBody] ExamTTTransSettingsDTO id)
        {
            // id = 12;
            return _inter.deactivate(id);
        }
        [Route("getdetails")]
        public ExamTTTransSettingsDTO Getdetails([FromBody] ExamTTTransSettingsDTO data)
        {
            return _inter.getdetails(data);
        }
        [Route("editgetdetails")]
        public ExamTTTransSettingsDTO editgetdetails([FromBody] ExamTTTransSettingsDTO data)
        {
            return _inter.editgetdetails(data);
        }

        [Route("onselectAcdYear")]
        public ExamTTTransSettingsDTO onselectAcdYear([FromBody] ExamTTTransSettingsDTO data)
        {
            return _inter.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public ExamTTTransSettingsDTO onselectclass([FromBody] ExamTTTransSettingsDTO data)
        {
            return _inter.onselectclass(data);
        }

        [Route("onselectSection")]
        public ExamTTTransSettingsDTO onselectSection([FromBody] ExamTTTransSettingsDTO data)
        {
            return _inter.onselectSection(data);
        }
        [Route("onselectSubject")]
        public ExamTTTransSettingsDTO onselectSubject([FromBody] ExamTTTransSettingsDTO data)
        {
            return _inter.onselectSubject(data);
        }

        [Route("onselectSubSubject")]
        public ExamTTTransSettingsDTO onselectSubSubject([FromBody] ExamTTTransSettingsDTO data)
        {
            return _inter.onselectSubSubject(data);
        }

        [Route("savedetail")]
        public ExamTTTransSettingsDTO savedetail([FromBody] ExamTTTransSettingsDTO data)
        {
            return _inter.savedetail(data);
        }
        [Route("getalldetailsviewrecords/")]
        public ExamTTTransSettingsDTO getalldetailsviewrecords([FromBody]ExamTTTransSettingsDTO acdm)
        {

            return _inter.getalldetailsviewrecords(acdm);
        }
        public void Post([FromBody]string value)
        {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class ExamTTTransSettingsController : Controller
    {
        ExamTTTransSettingsDelegates _delobj = new ExamTTTransSettingsDelegates();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public ExamTTTransSettingsDTO getdetails(ExamTTTransSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }

        [Route("editgetdetails/{id:int}")]
        public ExamTTTransSettingsDTO editgetdetails(int id)
        {
            ExamTTTransSettingsDTO dto = new ExamTTTransSettingsDTO();
            dto.EXTT_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.editgetdetails(dto);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public ExamTTTransSettingsDTO getalldetailsviewrecords(int id)
        {
            ExamTTTransSettingsDTO dto = new ExamTTTransSettingsDTO();
            dto.EXTT_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getalldetailsviewrecords(dto);
        }
        // POST api/values
        [HttpPost]
        [Route("deactivate")]
        public ExamTTTransSettingsDTO deactvate([FromBody] ExamTTTransSettingsDTO id)
        {
            return _delobj.deactivaterecord(id);
        }

        [Route("onselectAcdYear")]
        public ExamTTTransSettingsDTO onselectAcdYear([FromBody]ExamTTTransSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public ExamTTTransSettingsDTO onselectclass([FromBody]ExamTTTransSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectclass(data);
        }

        [Route("onselectSection")]
        public ExamTTTransSettingsDTO onselectSection([FromBody]ExamTTTransSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectSection(data);
        }
        [Route("onselectSubject")]
        public ExamTTTransSettingsDTO onselectSubject([FromBody]ExamTTTransSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectSubject(data);
        }
        [Route("onselectSubSubject")]
        public ExamTTTransSettingsDTO onselectSubSubject([FromBody]ExamTTTransSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectSubSubject(data);
        }
        [Route("savedetail")]
        public ExamTTTransSettingsDTO savedetail([FromBody]ExamTTTransSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.savedetail(data);
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

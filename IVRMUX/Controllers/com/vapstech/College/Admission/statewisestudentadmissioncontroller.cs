using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class statewisestudentadmissioncontroller : Controller
    {
        statewisestudentadmissionDelegate _delobj = new statewisestudentadmissionDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public statewisestudentadmissionDTO getdetails(statewisestudentadmissionDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }

        // POST api/values
        [HttpPost]              

        [Route("onselectAcdYear")]
        public statewisestudentadmissionDTO onselectAcdYear([FromBody]statewisestudentadmissionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectAcdYear(data);
        }

        [Route("onselectCourse")]
        public statewisestudentadmissionDTO onselectCourse([FromBody]statewisestudentadmissionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectCourse(data);
        }

        [Route("onselectBranch")]
        public statewisestudentadmissionDTO onselectBranch([FromBody]statewisestudentadmissionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectBranch(data);
        }

        [Route("onreport")]
        public statewisestudentadmissionDTO onreport([FromBody]statewisestudentadmissionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreport(data);
        }

        [Route("onreportcountry")]
        public statewisestudentadmissionDTO onreportcountry([FromBody]statewisestudentadmissionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreportcountry(data);
        }
        [Route("onreportreligionruralurban")]
        public statewisestudentadmissionDTO onreportreligionruralurban([FromBody]statewisestudentadmissionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreportreligionruralurban(data);
        }
        [Route("CategoryCasteWiseStudentReport")]
        public statewisestudentadmissionDTO CategoryCasteWiseStudentReport([FromBody]statewisestudentadmissionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.CategoryCasteWiseStudentReport(data);
        }
        [Route("onreportbirthday")]
        public statewisestudentadmissionDTO onreportbirthday([FromBody]statewisestudentadmissionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreportbirthday(data);
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

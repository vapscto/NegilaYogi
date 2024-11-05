﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class QuotaCategoryReportController : Controller
    {
        QuotaCategoryReportDelegate _delobj = new QuotaCategoryReportDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails/{id:int}")]
        public QuotaCategoryReportDTO getdetails(int id)
        {
            QuotaCategoryReportDTO data = new QuotaCategoryReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }
        [Route("onselectAcdYear")]
        public QuotaCategoryReportDTO onselectAcdYear([FromBody]QuotaCategoryReportDTO data)
        {
            //QuotaCategoryReportDTO data = new QuotaCategoryReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectAcdYear(data);
        }
        [Route("onselectCourse")]
        public QuotaCategoryReportDTO onselectCourse([FromBody]QuotaCategoryReportDTO data)
        {
            //QuotaCategoryReportDTO data = new QuotaCategoryReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectCourse(data);
        }
        [Route("onselectBranch")]
        public QuotaCategoryReportDTO onselectBranch([FromBody]QuotaCategoryReportDTO data)
        {
            //QuotaCategoryReportDTO data = new QuotaCategoryReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectBranch(data);
        }
        
        // POST api/values
        [HttpPost]
        [Route("onreport")]
        public QuotaCategoryReportDTO onreport([FromBody]QuotaCategoryReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreport(data);
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

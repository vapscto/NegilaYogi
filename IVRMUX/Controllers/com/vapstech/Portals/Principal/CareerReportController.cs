﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
//using DomainModel.Model.com.vapstech.p;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Portals.Employee;

//// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CareerReportController : Controller
    {
        CareerReportDelegates crStr = new CareerReportDelegates();
        // GET api/values
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
        // GET: api/Academic/5
        [HttpPost]
        [Route("getalldetails")]
        public CareerReportDTO getalldetails( [FromBody]  CareerReportDTO data)
        {
           
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
          
            return crStr.getalldetails(data);
        }
        [HttpPost]
        [Route("ondatechange")]
        public CareerReportDTO ondatechange([FromBody] CareerReportDTO data)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            return crStr.getalldetails(data);
        }

        //==========================home/class work upload

        [Route("get_home_classwork")]
        public IVRM_Homework_DTO get_home_classwork([FromBody] IVRM_Homework_DTO data)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            return crStr.get_home_classwork(data);
        }
    }
}



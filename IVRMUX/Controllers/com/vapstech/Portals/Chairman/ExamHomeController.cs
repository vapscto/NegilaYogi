
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
//using DomainModel.Model.com.vapstech.p;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ExamHomeController : Controller
    {


        ExamHomeDelegates crStr = new ExamHomeDelegates();


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
        [HttpGet]
        [Route("Getdetails")]
        public ExamHomeDTO Getdetails(ExamHomeDTO data)
        {
         data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return crStr.Getdetails(data);            
        }


        [HttpGet]
        [Route("getcategory/{id}")]
        public ExamHomeDTO getcategory(int id)
        {
            ExamHomeDTO data = new ExamHomeDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            
            return crStr.getcategory(data);
        }
        [HttpPost]
        [Route("getclassexam")]
        public  ExamHomeDTO getclassexam([FromBody] ExamHomeDTO data)
        {
           data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.getclassexam(data);

        }


        [HttpPost]
        [Route("showreport")]
        public ExamHomeDTO showreport([FromBody] ExamHomeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return crStr.showreport(data);
        }

        [HttpPost]
        [Route("showsectioncount")]
        public ExamHomeDTO showsectioncount([FromBody] ExamHomeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return crStr.showsectioncount(data);

        }
        
    }

}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.COE;
using PreadmissionDTOs.com.vaps.COE;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class exammastercategoryController : Controller
    {
        exammastercategoryDelegates objdelegate = new exammastercategoryDelegates();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public exammastercategoryDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            exammastercategoryDTO obj= objdelegate.getdetails(id);
            obj.ASMAY_Id= Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return obj;
            //return objdelegate.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail1")]
        public exammastercategoryDTO savedetail1([FromBody] exammastercategoryDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail1(categorypage);
        }
        [HttpPost]
        [Route("savedetail2")]
        public exammastercategoryDTO savedetail2([FromBody] exammastercategoryDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail2(categorypage);
        }
        [Route("getalldetailsviewrecords")]
        public exammastercategoryDTO getalldetailsviewrecords([FromBody] exammastercategoryDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getalldetailsviewrecords(categorypage);
        }
        [Route("deactivate_sec")]
        public exammastercategoryDTO deactivate_sec([FromBody] exammastercategoryDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate_sec(categorypage);
        }
        [HttpPost]
        [Route("deactivate1")]
        public exammastercategoryDTO deactivate1([FromBody] exammastercategoryDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate1(categorypage);
        }
        [HttpPost]
        [Route("deactivate2")]
        public exammastercategoryDTO deactivate2([FromBody] exammastercategoryDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate2(categorypage);
        }

        [Route("geteventdetails")]
        public exammastercategoryDTO geteventdetails([FromBody] exammastercategoryDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.geteventdetails(categorypage);

        }
        [Route("getalldetailsviewrecords1/{id:int}")]
        public exammastercategoryDTO getalldetailsviewrecords1(int id)
        {

            return objdelegate.getalldetailsviewrecords1(id);
        }
        [Route("getalldetailsviewrecords2/{id:int}")]
        public exammastercategoryDTO getalldetailsviewrecords2(int id)
        {

            return objdelegate.getalldetailsviewrecords2(id);
        }
        [Route("getdetails1/{id:int}")]
        public exammastercategoryDTO getdetail1(int id)
        {
            return objdelegate.getpagedetails1(id);

        }
        [Route("getdetails2")]
        public exammastercategoryDTO getdetail2([FromBody] exammastercategoryDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getpagedetails2(categorypage);

        }

        [Route("get_cate_class")]
        public exammastercategoryDTO getdetail3([FromBody] exammastercategoryDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getpagedetails3(categorypage);

        }
        [Route("get_cls_sections")]
        public exammastercategoryDTO get_cls_sections([FromBody] exammastercategoryDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_cls_sections(categorypage);

        }
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public exammastercategoryDTO deletepages(int id)
        {
            return objdelegate.deleterec(id);
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

        [Route("Save_ReportCard_Format")]
        public exammastercategoryDTO Save_ReportCard_Format([FromBody] exammastercategoryDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate.Save_ReportCard_Format(categorypage);
        }

        [Route("deactive_format")]
        public exammastercategoryDTO deactive_format([FromBody] exammastercategoryDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate.deactive_format(categorypage);
        }
    }
}

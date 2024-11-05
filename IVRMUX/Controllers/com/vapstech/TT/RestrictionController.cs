using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.TT
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class RestrictionController : Controller
    {
        RestrictionDelegate objdelegate = new RestrictionDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public TTRestrictionDTO Get([FromQuery] int id)
        {
            TTRestrictionDTO data = new TTRestrictionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return objdelegate.getdetails(data);
        }

        [HttpPost]
        [Route("savedetail1")]
        public TTRestrictionDTO savedetail1([FromBody] TTRestrictionDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail1(categorypage);
        }
        [HttpPost]
        [Route("savedetail2")]
        public TTRestrictionDTO savedetail2([FromBody] TTRestrictionDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail2(categorypage);
        }
        [HttpPost]
        [Route("savedetail3")]
        public TTRestrictionDTO savedetail3([FromBody] TTRestrictionDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail3(categorypage);
        }
        [HttpPost]
        [Route("savedetail4")]
        public TTRestrictionDTO savedetail4([FromBody] TTRestrictionDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail4(categorypage);
        }
        [HttpPost]
        [Route("savedetail5")]
        public TTRestrictionDTO savedetail5([FromBody] TTRestrictionDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail5(categorypage);
        }
        [Route("getpagedetails1/{id:int}")]
        public TTRestrictionDTO getpagedetails1(int id)
        {
            return objdelegate.getpagedetails1(id);

        }
        [Route("getpagedetails2/{id:int}")]
        public TTRestrictionDTO getpagedetails2(int id)
        {
            return objdelegate.getpagedetails2(id);

        }
        [Route("getpagedetails3/{id:int}")]
        public TTRestrictionDTO getpagedetails3(int id)
        {
            return objdelegate.getpagedetails3(id);

        }
        [Route("getpagedetails4/{id:int}")]
        public TTRestrictionDTO getpagedetails4(int id)
        {
            return objdelegate.getpagedetails4(id);

        }
        [Route("getpagedetails5/{id:int}")]
        public TTRestrictionDTO getpagedetails5(int id)
        {
            return objdelegate.getpagedetails5(id);

        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public TTRestrictionDTO getalldetailsviewrecords(int id)
        {

            return objdelegate.getalldetailsviewrecords(id);
        }

        [HttpPost]
        [Route("get_catg")]
        public TTRestrictionDTO getcategories([FromBody] TTRestrictionDTO data)
        {
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getcategories(data);
        }

        // POST api/values
        [HttpPost]
        [Route("getclass_catg")]
        public TTRestrictionDTO getclasses([FromBody] TTRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getclasses(data);
        }
        [HttpPost]
        [Route("getperiod_class")]
        public TTRestrictionDTO getperiods([FromBody] TTRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getperiods(data);
        }
        [HttpPost]
        [Route("getstaff_section")]
        public TTRestrictionDTO getstaff([FromBody] TTRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getstaff(data);
        }
        [HttpPost]
        [Route("getsubject_staff")]
        public TTRestrictionDTO getsubjects([FromBody] TTRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getsubjects(data);
        }
        [HttpPost]
        [Route("get_cls_sec_subbystaff")]
        public TTRestrictionDTO get_cls_sec_subs([FromBody] TTRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_cls_sec_subs(data);
        }
        [HttpPost]
        [Route("get_cls_sec_staffbysub")]
        public TTRestrictionDTO get_cls_sec_staffs([FromBody] TTRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_cls_sec_staffs(data);
        }
        [Route("getalldetailsviewrecords2/{id:int}")]
        public TTRestrictionDTO getalldetailsviewrecords2(int id)
        {

            return objdelegate.getalldetailsviewrecords2(id);
        }
        [Route("getalldetailsviewrecords3/{id:int}")]
        public TTRestrictionDTO getalldetailsviewrecords3(int id)
        {

            return objdelegate.getalldetailsviewrecords3(id);
        }
        [Route("getalldetailsviewrecords4/{id:int}")]
        public TTRestrictionDTO getalldetailsviewrecords4(int id)
        {

            return objdelegate.getalldetailsviewrecords4(id);
        }
        [Route("getalldetailsviewrecords5/{id:int}")]
        public TTRestrictionDTO getalldetailsviewrecords5(int id)
        {

            return objdelegate.getalldetailsviewrecords5(id);
        }

        [HttpPost]
        [Route("deactivate1")]
        public TTRestrictionDTO deactivate1([FromBody] TTRestrictionDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate1(categorypage);
        }
        [HttpPost]
        [Route("deactivate2")]
        public TTRestrictionDTO deactivate2([FromBody] TTRestrictionDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate2(categorypage);
        }
        [HttpPost]
        [Route("deactivate3")]
        public TTRestrictionDTO deactivate3([FromBody] TTRestrictionDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate3(categorypage);
        }
        [HttpPost]
        [Route("deactivate4")]
        public TTRestrictionDTO deactivate4([FromBody] TTRestrictionDTO categorypage)
        {
           categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate4(categorypage);
        }
        [HttpPost]
        [Route("deactivate5")]
        public TTRestrictionDTO deactivate5([FromBody] TTRestrictionDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate5(categorypage);
        }
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail")]
        public TTRestrictionDTO savedetail([FromBody] TTRestrictionDTO periodpage)
        {
           
            periodpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(periodpage);
        }
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public TTRestrictionDTO deletepages(int id)
        {
            return objdelegate.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public TTRestrictionDTO deactivate([FromBody] TTRestrictionDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate(categorypage);
        }
        [Route("getdetails/{id:int}")]
        public TTRestrictionDTO getdetail(int id)
        {
            return objdelegate.getpagedetails(id);

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

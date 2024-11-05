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
    public class FixingController : Controller
    {
        FixingDelegate objdelegate = new FixingDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public TTFixingDTO Get([FromQuery] int id)
        {
            TTFixingDTO data = new TTFixingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return objdelegate.getdetails(data);
        }

        [HttpPost]
        [Route("savedetail1")]
        public TTFixingDTO savedetail1([FromBody] TTFixingDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail1(categorypage);
        }
        [HttpPost]
        [Route("savedetail2")]
        public TTFixingDTO savedetail2([FromBody] TTFixingDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail2(categorypage);
        }
        [HttpPost]
        [Route("savedetail3")]
        public TTFixingDTO savedetail3([FromBody] TTFixingDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail3(categorypage);
        }
        [HttpPost]
        [Route("savedetail4")]
        public TTFixingDTO savedetail4([FromBody] TTFixingDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail4(categorypage);
        }
        [HttpPost]
        [Route("savedetail5")]
        public TTFixingDTO savedetail5([FromBody] TTFixingDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail5(categorypage);
        }
        [Route("getpagedetails1/{id:int}")]
        public TTFixingDTO getpagedetails1(int id)
        {
            return objdelegate.getpagedetails1(id);

        }
        [Route("getpagedetails2/{id:int}")]
        public TTFixingDTO getpagedetails2(int id)
        {
            return objdelegate.getpagedetails2(id);

        }
        [Route("getpagedetails3/{id:int}")]
        public TTFixingDTO getpagedetails3(int id)
        {
            return objdelegate.getpagedetails3(id);

        }
        [Route("getpagedetails4/{id:int}")]
        public TTFixingDTO getpagedetails4(int id)
        {
            return objdelegate.getpagedetails4(id);

        }
        [Route("getpagedetails5/{id:int}")]
        public TTFixingDTO getpagedetails5(int id)
        {
            return objdelegate.getpagedetails5(id);

        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public TTFixingDTO getalldetailsviewrecords(int id)
        {

            return objdelegate.getalldetailsviewrecords(id);
        }

        [HttpPost]
        [Route("get_catg")]
        public TTFixingDTO getcategories([FromBody] TTFixingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getcategories(data);
        }

        // POST api/values
        [HttpPost]
        [Route("getclass_catg")]
        public TTFixingDTO getclasses([FromBody] TTFixingDTO data)
        {  
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getclasses(data);
        }
        [HttpPost]
        [Route("getperiod_class")]
        public TTFixingDTO getperiods([FromBody] TTFixingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getperiods(data);
        }
        [HttpPost]
        [Route("getstaff_section")]
        public TTFixingDTO getstaff([FromBody] TTFixingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getstaff(data);
        }
        [HttpPost]
        [Route("getsubject_staff")]
        public TTFixingDTO getsubjects([FromBody] TTFixingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getsubjects(data);
        }
        [HttpPost]
        [Route("get_cls_sec_subbystaff")]
        public TTFixingDTO get_cls_sec_subs([FromBody] TTFixingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_cls_sec_subs(data);
        }
        [HttpPost]
        [Route("get_cls_sec_staffbysub")]
        public TTFixingDTO get_cls_sec_staffs([FromBody] TTFixingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_cls_sec_staffs(data);
        }
        [Route("getalldetailsviewrecords2/{id:int}")]
        public TTFixingDTO getalldetailsviewrecords2(int id)
        {

            return objdelegate.getalldetailsviewrecords2(id);
        }
        [Route("getalldetailsviewrecords3/{id:int}")]
        public TTFixingDTO getalldetailsviewrecords3(int id)
        {

            return objdelegate.getalldetailsviewrecords3(id);
        }
        [Route("getalldetailsviewrecords4/{id:int}")]
        public TTFixingDTO getalldetailsviewrecords4(int id)
        {

            return objdelegate.getalldetailsviewrecords4(id);
        }
        [Route("getalldetailsviewrecords5/{id:int}")]
        public TTFixingDTO getalldetailsviewrecords5(int id)
        {

            return objdelegate.getalldetailsviewrecords5(id);
        }

        [HttpPost]
        [Route("deactivate1")]
        public TTFixingDTO deactivate1([FromBody] TTFixingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate1(categorypage);
        }
        [HttpPost]
        [Route("deactivate2")]
        public TTFixingDTO deactivate2([FromBody] TTFixingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate2(categorypage);
        }
        [HttpPost]
        [Route("deactivate3")]
        public TTFixingDTO deactivate3([FromBody] TTFixingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate3(categorypage);
        }
        [HttpPost]
        [Route("deactivate4")]
        public TTFixingDTO deactivate4([FromBody] TTFixingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate4(categorypage);
        }
        [HttpPost]
        [Route("deactivate5")]
        public TTFixingDTO deactivate5([FromBody] TTFixingDTO categorypage)
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
        public TTFixingDTO savedetail([FromBody] TTFixingDTO periodpage)
        {
           
            periodpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(periodpage);
        }
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public TTFixingDTO deletepages(int id)
        {
            return objdelegate.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public TTFixingDTO deactivate([FromBody] TTFixingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate(categorypage);
        }
        [Route("getdetails/{id:int}")]
        public TTFixingDTO getdetail(int id)
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

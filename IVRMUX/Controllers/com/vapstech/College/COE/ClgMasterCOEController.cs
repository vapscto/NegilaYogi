using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.COE;
using PreadmissionDTOs.com.vaps.COE;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.COE;
using PreadmissionDTOs.com.vaps.College.COE;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.COE
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgMasterCOEController : Controller
    {
        ClgMasterCOEDelegate objdelegate = new ClgMasterCOEDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public ClgMasterCOEDTO getalldetails(int id)
        {
            ClgMasterCOEDTO categorypage = new ClgMasterCOEDTO();
            categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(categorypage);
        }
        //[Route("getalldetails")]
        //public ClgMasterCOEDTO Get([FromQuery] int id)

        //{
        //    id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return objdelegate.getdetails(id);
        //}
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("courseselect")]
        public ClgMasterCOEDTO courseselect([FromBody] ClgMasterCOEDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.courseselect(categorypage);
        }
        [Route("branchselect")]
        public ClgMasterCOEDTO branchselect([FromBody] ClgMasterCOEDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.branchselect(categorypage);
        }

        [HttpPost]
        [Route("savedetail1")]
        public ClgMasterCOEDTO savedetail1([FromBody] ClgMasterCOEDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail1(categorypage);
        }
        [HttpPost]
        [Route("savedetail2")]
        public ClgMasterCOEDTO savedetail2([FromBody] ClgMasterCOEDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail2(categorypage);
        }

        [HttpPost]
        [Route("deactivate1")]
        public ClgMasterCOEDTO deactivate1([FromBody] ClgMasterCOEDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate1(categorypage);
        }
        [HttpPost]
        [Route("deactivate2")]
        public ClgMasterCOEDTO deactivate2([FromBody] ClgMasterCOEDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate2(categorypage);
        }

        [Route("geteventdetails")]
        public ClgMasterCOEDTO geteventdetails([FromBody] ClgMasterCOEDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.geteventdetails(categorypage);

        }
        [Route("getalldetailsviewrecords1/{id:int}")]
        public ClgMasterCOEDTO getalldetailsviewrecords1(int id)
        {

            return objdelegate.getalldetailsviewrecords1(id);
        }
        [Route("getalldetailsviewrecords2/{id:int}")]
        public ClgMasterCOEDTO getalldetailsviewrecords2(int id)
        {
            ClgMasterCOEDTO categorypage = new ClgMasterCOEDTO();
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getalldetailsviewrecords2(id);
        }
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public ClgMasterCOEDTO deletepages(int id)
        {
            return objdelegate.deleterec(id);
        }
        [Route("getdetails1/{id:int}")]
        public ClgMasterCOEDTO getdetail1(int id)
        {
            return objdelegate.getpagedetails1(id);

        }
        [Route("getdetails2/{id:int}")]
        public ClgMasterCOEDTO getdetail2(int id)
        {
            return objdelegate.getpagedetails2(id);

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

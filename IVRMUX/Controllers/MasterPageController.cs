using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterPageController : Controller
    {
        MasterPageDelegate od = new MasterPageDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterPageDTO Get([FromQuery] int id)
        {
            return od.getdetails(id);
        }



      
        [Route("getalldetailsmobile/{id:int}")]
        public MasterPageDTO getalldetailsmobile ([FromQuery] int id)
        {
            return od.getalldetailsmobile(id);
        }

        [Route("getdetails/{id:int}")]
        public MasterPageDTO getdetail(int id)
        {
            return od.getpagedetails(id);
        }

        [Route("mobilegetdetails/{id:int}")]
        public MasterPageDTO mobilegetdetails(int id)
        {
            return od.mobilegetdetails(id);
        }


        // POST api/values
        [HttpPost]
       
        public MasterPageDTO savedetail([FromBody] MasterPageDTO maspage)
        {
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            maspage.userid = UserId;
            maspage.IVRMP_TemplateFlag = true;
            return od.savedetails(maspage);
        }

        [Route("mobilesaveorgdet")]
        public MasterPageDTO mobilesaveorgdet([FromBody] MasterPageDTO maspage)
        {
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            maspage.userid = UserId;
            maspage.IVRMP_TemplateFlag = true;
            return od.mobilesaveorgdet(maspage);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpPost("{id}")]
        public MasterPageDTO Put(int id, [FromBody]MasterPageDTO value)
        {
            return od.getsearchdata(id, value);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public MasterPageDTO Delete(int id)
        {
            return od.deleterec(id);
        }

        [Route("mobiledeleterec")]
        public MasterPageDTO mobiledeleterec([FromBody] MasterPageDTO id)
        {
            return od.mobiledeleterec(id);
        }
    }
}

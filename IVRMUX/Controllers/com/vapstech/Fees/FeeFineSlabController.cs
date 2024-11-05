using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeFineSlabController : Controller
    {
        FeeFineSlabDelegate _feeFS = new FeeFineSlabDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeFineSlabDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _feeFS.getdetails(id);
        }
        //for edit
        [Route("getdetails/{id:int}")]
        public FeeFineSlabDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return _feeFS.getpagedetails(id);
        }
        [Route("Editdetails/{id:int}")]
        public FeeFineSlabDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return _feeFS.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        [Route("savedetail/")]
        public FeeFineSlabDTO savedetail([FromBody] FeeFineSlabDTO GroupHeadpage)
        {
            GroupHeadpage.MI_Id =Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _feeFS.savedetails(GroupHeadpage);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public FeeFineSlabDTO Delete(int id)
        {
            return _feeFS.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public FeeFineSlabDTO deactvate([FromBody] FeeFineSlabDTO id)
        {
            return _feeFS.deactivateAcademicYear(id);
        }
    }
}

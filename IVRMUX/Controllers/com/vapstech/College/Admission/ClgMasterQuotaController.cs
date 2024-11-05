using System;
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
    public class ClgMasterQuotaController : Controller
    {

        ClgMasterQuotaDelegate _Quota = new ClgMasterQuotaDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [Route("getalldetails/{id:int}")]
        public ClgQuotaDTO getalldetails (int id)
        {
            ClgQuotaDTO data = new ClgQuotaDTO();
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Quota.getalldetails(data);
        }

        //-----------------------------------Master Quota 
        [Route("savedetails")]
        public ClgQuotaDTO savedetails([FromBody] ClgQuotaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Quota.savedetails(data);
        }
        [Route("activedeactiveQuota")]
        public ClgQuotaDTO activedeactiveQuota([FromBody] ClgQuotaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Quota.activedeactiveQuota(data);
        }
        [Route("editdetails")]
        public ClgQuotaDTO editdetails([FromBody] ClgQuotaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Quota.editdetails(data);
        }

        //-----------------------------------Master Quota Category 
        [Route("savedetails1")]
        public ClgQuotaDTO savedetails1([FromBody] ClgQuotaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Quota.savedetails1(data);
        }
        [Route("activedeactiveQuota1")]
        public ClgQuotaDTO activedeactiveQuota1([FromBody] ClgQuotaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Quota.activedeactiveQuota1(data);
        }
        [Route("editdetails1")]
        public ClgQuotaDTO editdetails1([FromBody] ClgQuotaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Quota.editdetails1(data);
        }
        //-----------------------------------Master Quota category Mapping
        [Route("savedetails2")]
        public ClgQuotaDTO savedetails2([FromBody] ClgQuotaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Quota.savedetails2(data);
        }
        [Route("activedeactiveQuota2")]
        public ClgQuotaDTO activedeactiveQuota2([FromBody] ClgQuotaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Quota.activedeactiveQuota2(data);
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
    }
}

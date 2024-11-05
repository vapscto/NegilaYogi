using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class ClgMasterQuotaFacadeController : Controller
    {
        public ClgMasterQuotaInterface _Quotaint;

        public ClgMasterQuotaFacadeController(ClgMasterQuotaInterface Quotaintf)
        {
            _Quotaint = Quotaintf;
        }
        // GET: api/values
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


        [Route("getalldetails")]
        public ClgQuotaDTO getalldetails([FromBody] ClgQuotaDTO data)
        {
            return _Quotaint.getalldetails(data);
        }


        //------------------------------------------Master Quota
        [HttpPost]
        [Route("savedetails")]
        public ClgQuotaDTO Post([FromBody] ClgQuotaDTO data)
        {
            return _Quotaint.savedetails(data);
        }

        [Route("editdetails")]
        public ClgQuotaDTO editdetails([FromBody] ClgQuotaDTO data)
        {
            return _Quotaint.editdetails(data);
        }

        [Route("activedeactiveQuota")]
        public ClgQuotaDTO activedeactiveQuota([FromBody] ClgQuotaDTO data)
        {
            return _Quotaint.activedeactiveQuota(data);
        }

        //------------------------------------------Master Quota Category
        [HttpPost]
        [Route("savedetails1")]
        public ClgQuotaDTO savedetails1([FromBody] ClgQuotaDTO data)
        {
            return _Quotaint.savedetails1(data);
        }

        [Route("editdetails1")]
        public ClgQuotaDTO editdetails1([FromBody] ClgQuotaDTO data)
        {
            return _Quotaint.editdetails1(data);
        }

        [Route("activedeactiveQuota1")]
        public ClgQuotaDTO activedeactiveQuota1([FromBody] ClgQuotaDTO data)
        {
            return _Quotaint.activedeactiveQuota1(data);
        }

        //------------------------------------------Master Quota Category Mapping
        [HttpPost]
        [Route("savedetails2")]
        public ClgQuotaDTO savedetails2([FromBody] ClgQuotaDTO data)
        {
            return _Quotaint.savedetails2(data);
        }

        [Route("activedeactiveQuota2")]
        public ClgQuotaDTO activedeactiveQuota2([FromBody] ClgQuotaDTO data)
        {
            return _Quotaint.activedeactiveQuota2(data);
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

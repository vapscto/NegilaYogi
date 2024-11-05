using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.IVRS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.IVRS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.IVRS
{
    [Route("api/[controller]")]
    public class IVRSRechargeController : Controller
    {
        IVRSRechargeDelegate TCD = new IVRSRechargeDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("getalldetails")]
        public IVRS_Acc_RechargeDTO Get([FromQuery] int id)
        {
            IVRS_Acc_RechargeDTO data = new IVRS_Acc_RechargeDTO();
            return TCD.getdetails(data);
        }
        [HttpPost]
        [Route("savedetail")]
        public IVRS_Acc_RechargeDTO savedetail([FromBody] IVRS_Acc_RechargeDTO page1)
        {
            return TCD.savedetail(page1);
        }
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [Route("getdetails_page/{id:int}")]
        public IVRS_Acc_RechargeDTO getdetails_page(int id)
        {
            return TCD.getdetails_page(id);

        }
        [HttpPost]
        [Route("deactivate")]
        public IVRS_Acc_RechargeDTO deactvate([FromBody] IVRS_Acc_RechargeDTO id)
        {
            return TCD.deactivate(id);
        }
    }
}

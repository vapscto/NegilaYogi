using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.IVRS.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.IVRS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.IVRS.Controllers
{
    [Route("api/[controller]")]
    public class IVRSRechargeFacadeController : Controller
    {
        public IVRSRechargeInterface _intf;
        public IVRSRechargeFacadeController(IVRSRechargeInterface intf)
        {
            _intf = intf;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        [Route("getdetails")]
        public IVRS_Acc_RechargeDTO getdetails([FromBody] IVRS_Acc_RechargeDTO data)
        {
            return _intf.getdetails(data);
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("savedetail")]
        public IVRS_Acc_RechargeDTO savedetail([FromBody] IVRS_Acc_RechargeDTO data)
        {
            return _intf.savedetail(data);
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
            return _intf.getdetails_page(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public IVRS_Acc_RechargeDTO deactivateAcdmYear([FromBody] IVRS_Acc_RechargeDTO id)
        {
            return _intf.deactivate(id);
        }
    }
}
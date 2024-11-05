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
    public class IvrshomeController : Controller
    {

        IvrsDelegate TCD = new IvrsDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [HttpGet]
        [Route("getalldetails")]
        public IVRSDTO Get([FromQuery] int id)
        {
            IVRSDTO data = new IVRSDTO();
            return TCD.getdetails(data);
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }


        [HttpPost]
        [Route("savedetail")]
        public IVRM_IVRS_ConfigurationDTO savedetail([FromBody] IVRM_IVRS_ConfigurationDTO page1)
        {
            return TCD.savedetail(page1);
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
        [Route("getdetails/{id:int}")]
        public IVRSDTO getdetail(int id)
        {
            return TCD.getpagedetails(id);

        }
        [Route("getdetails_page/{id:int}")]
        public IVRM_IVRS_ConfigurationDTO getdetails_page(int id)
        {
            return TCD.getdetails_page(id);

        }
        [HttpPost]
        [Route("deactivate")]
        public IVRM_IVRS_ConfigurationDTO deactvate([FromBody] IVRM_IVRS_ConfigurationDTO id)
        {
            return TCD.deactivate(id);
        }
    }
}

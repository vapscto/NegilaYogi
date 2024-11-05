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
    public class IVRSMasterLanguagesController : Controller
    {
        IVRSMasterLanguagesDelegate TCD = new IVRSMasterLanguagesDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("getalldetails")]
        public IVRS_Master_LanguagesDTO Get([FromQuery] int id)
        {
            IVRS_Master_LanguagesDTO data = new IVRS_Master_LanguagesDTO();
            return TCD.getdetails(data);
        }
    
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail")]
        public IVRS_Master_LanguagesDTO savedetail([FromBody] IVRS_Master_LanguagesDTO page1)
        {
            page1.IMLA_CreatedBy= HttpContext.Session.GetInt32("UserId").ToString();
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
        public IVRS_Master_LanguagesDTO getdetails_page(int id)
        {
            return TCD.getdetails_page(id);

        }
        [HttpPost]
        [Route("deactivate")]
        public IVRS_Master_LanguagesDTO deactvate([FromBody] IVRS_Master_LanguagesDTO id)
        {
            return TCD.deactivate(id);
        }
    }
}

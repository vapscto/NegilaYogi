using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontOfficeHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterTimeSettingFacade : Controller
    {
        public MasterTimeSettingInterface _ttbreaktime;


        public MasterTimeSettingFacade(MasterTimeSettingInterface maspag)
        {
            _ttbreaktime = maspag;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public MasterTimeSettingDTO getpagedetails(int id)
        {
            return _ttbreaktime.getdetails(id);
        }
        // POST api/values

        [HttpPost]
        [Route("savedetail")]
        public MasterTimeSettingDTO Post([FromBody] MasterTimeSettingDTO org)
        {
            return _ttbreaktime.savedetail(org);
        }
        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterTimeSettingDTO getordetails(int id)
        {
            // id = 12;
            return _ttbreaktime.getpageedit(id);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterTimeSettingDTO Deleterec(int id)
        {
            return _ttbreaktime.deleterec(id);
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

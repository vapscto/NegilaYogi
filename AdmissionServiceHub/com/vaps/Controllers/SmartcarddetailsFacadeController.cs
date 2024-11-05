using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using AdmissionServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SmartcarddetailsFacadeController : Controller
    {
        public SmartcarddetailsInterface _AttenRpt;
        public SmartcarddetailsFacadeController(SmartcarddetailsInterface AttenRpt)
        {
            _AttenRpt = AttenRpt;
        }
        // load initial dropdown
        [Route("getinitialdata/{mi_id:int}")]
        public Task<Adm_M_StudentDTO> getinitialdata(int mi_id)
        {
            return _AttenRpt.getInitailData(mi_id);
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

        // POST api/values
        [HttpPost]
        [Route("searchdata")]
        public Task<Adm_M_StudentDTO> searchdata([FromBody] Adm_M_StudentDTO data)
        {
            return _AttenRpt.getserdata(data);
        }
        [HttpPost]
        [Route("getstudentdetails")]
        public Task<Adm_M_StudentDTO> getstudentdetails([FromBody] Adm_M_StudentDTO data)
        {
            return _AttenRpt.getstudentdetails(data);
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

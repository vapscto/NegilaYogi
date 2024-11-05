using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTableServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CLGTTGenerationFacadeController : Controller
    {
        public CLGTTGenerationInterface _ttMain;

        public CLGTTGenerationFacadeController(CLGTTGenerationInterface maspag)
        {
            _ttMain = maspag;
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
        [Route("generate")]
        public CLGTTGenerationDTO Post([FromBody] CLGTTGenerationDTO org)
        {
            return _ttMain.generate(org);
        }
        [Route("get_catg")]
        public CLGTTGenerationDTO get_catg([FromBody] CLGTTGenerationDTO org)
        {
            return _ttMain.get_catg(org);
        }
        [Route("get_count")]
        public CLGTTGenerationDTO get_count([FromBody] CLGTTGenerationDTO org)
        {
            return _ttMain.get_count(org);
        }
        [Route("resetTT")]
        public CLGTTGenerationDTO resetTT([FromBody] CLGTTGenerationDTO org)
        {
            return _ttMain.resetTT(org);
        }
        [Route("Get_temp_data")]
        public CLGTTGenerationDTO Get_temp_data([FromBody] CLGTTGenerationDTO org)
        {
            return _ttMain.Get_temp_data(org);
        }
        [Route("getalldetailsviewrecords")]
        public CLGTTGenerationDTO getalldetailsviewrecords([FromBody] CLGTTGenerationDTO org)
        {
            return _ttMain.getalldetailsviewrecords(org);
        }
        [Route("getreplacemntdetailsviewrecords")]
        public CLGTTGenerationDTO getreplacemntdetailsviewrecords([FromBody] CLGTTGenerationDTO org)
        {
            return _ttMain.getreplacemntdetailsviewrecords(org);
        }
        [Route("saveTemptomain")]
        public CLGTTGenerationDTO saveTemptomain([FromBody] CLGTTGenerationDTO org)
        {
            return _ttMain.saveTemptomain(org);
        }
        [Route("getdetails")]
        public CLGTTGenerationDTO getdetails([FromBody] CLGTTGenerationDTO data)
        {
            return _ttMain.getdetails(data);
        }
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [Route("deactivate")]
        public CLGTTGenerationDTO deactivate([FromBody] CLGTTGenerationDTO data)
        {
            return _ttMain.deactivate(data);
        }
    }
}

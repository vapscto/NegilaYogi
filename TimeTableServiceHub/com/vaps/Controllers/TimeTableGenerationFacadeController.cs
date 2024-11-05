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
    public class TimeTableGenerationFacadeController : Controller
    {
        public TimeTableGenerationInterface _ttMain;

        public TimeTableGenerationFacadeController(TimeTableGenerationInterface maspag)
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
        public TT_Final_GenerationDTO Post([FromBody] TT_Final_GenerationDTO org)
        {
            return _ttMain.generate(org);
        }
        [Route("get_catg")]
        public TT_Final_GenerationDTO get_catg([FromBody] TT_Final_GenerationDTO org)
        {
            return _ttMain.get_catg(org);
        }
        [Route("get_count")]
        public TT_Final_GenerationDTO get_count([FromBody] TT_Final_GenerationDTO org)
        {
            return _ttMain.get_count(org);
        }
        [Route("resetTT")]
        public TT_Final_GenerationDTO resetTT([FromBody] TT_Final_GenerationDTO org)
        {
            return _ttMain.resetTT(org);
        }
        [Route("Get_temp_data")]
        public TT_Final_GenerationDTO Get_temp_data([FromBody] TT_Final_GenerationDTO org)
        {
            return _ttMain.Get_temp_data(org);
        }
        [Route("getalldetailsviewrecords")]
        public TT_Final_GenerationDTO getalldetailsviewrecords([FromBody] TT_Final_GenerationDTO org)
        {
            return _ttMain.getalldetailsviewrecords(org);
        }
        [Route("getreplacemntdetailsviewrecords")]
        public TT_Final_GenerationDTO getreplacemntdetailsviewrecords([FromBody] TT_Final_GenerationDTO org)
        {
            return _ttMain.getreplacemntdetailsviewrecords(org);
        }
        [Route("saveTemptomain")]
        public TT_Final_GenerationDTO saveTemptomain([FromBody] TT_Final_GenerationDTO org)
        {
            return _ttMain.saveTemptomain(org);
        }
        [Route("getdetails")]
        public TT_Final_GenerationDTO getdetails([FromBody] TT_Final_GenerationDTO data)
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
        public TT_Final_GenerationDTO deactivate([FromBody] TT_Final_GenerationDTO data)
        {
            return _ttMain.deactivate(data);
        }
    }
}

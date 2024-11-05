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
    public class CLGDeputationFacadeController : Controller
    {
        public CLGDeputationInterface _ttbreaktime;
        public CLGDeputationFacadeController(CLGDeputationInterface maspag)
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
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("get_period_alloted")]
        public CLGDeputationDTO get_period_alloted([FromBody] CLGDeputationDTO data)
        {
            return _ttbreaktime.get_period_alloted(data);
        }
        [Route("get_free_stfdets")]
        public CLGDeputationDTO get_free_stfdets([FromBody] CLGDeputationDTO data)
        {
            return _ttbreaktime.get_free_stfdets(data);
        }
        [Route("getalldetailsviewrecords2")]
        public CLGDeputationDTO getalldetailsviewrecords2([FromBody] CLGDeputationDTO data)
        {
            return _ttbreaktime.getalldetailsviewrecords2(data);
        }
        [Route("viewdeputation")]
        public CLGDeputationDTO viewdeputation([FromBody] CLGDeputationDTO data)
        {
            return _ttbreaktime.viewdeputation(data);
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

        [Route("getdetails")]
        public CLGDeputationDTO getorgdet([FromBody] CLGDeputationDTO data)
        {
            return _ttbreaktime.getdetails(data);
        }
       
        [Route("savedetails")]
        public CLGDeputationDTO savedetails([FromBody] CLGDeputationDTO org)
        {
            return _ttbreaktime.savedetails(org);
        }
        [Route("viewabsent")]
        public CLGDeputationDTO viewabsent([FromBody] CLGDeputationDTO org)
        {
            return _ttbreaktime.viewabsent(org);
        }
        [Route("getabsentstaff")]
        public CLGDeputationDTO getabsentstaff([FromBody] CLGDeputationDTO org)
        {
            return _ttbreaktime.getabsentstaff(org);
        }
       
    }
}

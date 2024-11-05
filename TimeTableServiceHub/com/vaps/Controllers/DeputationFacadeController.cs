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
    public class DeputationFacade : Controller
    {
        public DeputationInterface _ttbreaktime;
        public DeputationFacade(DeputationInterface maspag)
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
        public TTDeputationDTO get_period_alloted([FromBody] TTDeputationDTO org)
        {
            return _ttbreaktime.get_period_alloted(org);
        }
        [Route("get_free_stfdets")]
        public TTDeputationDTO get_free_stfdets([FromBody] TTDeputationDTO org)
        {
            return _ttbreaktime.get_free_stfdets(org);
        }
        [Route("getalldetailsviewrecords2")]
        public TTDeputationDTO getalldetailsviewrecords2([FromBody] TTDeputationDTO org)
        {
            return _ttbreaktime.getalldetailsviewrecords2(org);
        }
        [Route("viewrecordspopup9")]
        public TTDeputationDTO viewrecordspopup9([FromBody] TTDeputationDTO org)
        {
            return _ttbreaktime.viewrecordspopup9(org);
        }



        [Route("viewdeputation")]
        public TTDeputationDTO viewdeputation([FromBody] TTDeputationDTO org)
        {
            return _ttbreaktime.viewdeputation(org);
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
        public TTDeputationDTO getorgdet(int id)
        {
            return _ttbreaktime.getdetails(id);
        }
       
        [Route("savedetails")]
        public TTDeputationDTO savedetails([FromBody] TTDeputationDTO org)
        {
            return _ttbreaktime.savedetails(org);
        }
        [Route("viewabsent")]
        public TTDeputationDTO viewabsent([FromBody] TTDeputationDTO org)
        {
            return _ttbreaktime.viewabsent(org);
        }
        [Route("getabsentstaff")]
        public TTDeputationDTO getabsentstaff([FromBody] TTDeputationDTO org)
        {
            return _ttbreaktime.getabsentstaff(org);
        }
       
    }
}

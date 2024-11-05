using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using VisitorsManagementServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorsManagementServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class OutwardFacadeController : Controller
    {
        OutwardInterface interobj;
        public OutwardFacadeController(OutwardInterface obj)
        {
            interobj = obj;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        [Route("getDetails")]
        public OutwardDTO getDetails([FromBody]OutwardDTO data)
        {
            return interobj.getDetails(data);
        }
        [Route("EditDetails")]
        public OutwardDTO EditDetails([FromBody] OutwardDTO id)
        {
            return interobj.EditDetails(id);
        }
        [Route("saveData")]
        public OutwardDTO saveData([FromBody]OutwardDTO data)
        {
            return interobj.saveData(data);
        }
        [Route("deactivate")]
        public OutwardDTO deactivate([FromBody]OutwardDTO data)
        {
            return interobj.deactivate(data);
        }
        public void Post([FromBody]string value)
        {
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
    }
}

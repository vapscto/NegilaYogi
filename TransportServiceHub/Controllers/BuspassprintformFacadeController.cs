using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransportServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.Transport;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class BuspassprintformFacadeController : Controller
    {
        public BuspassprintformInterface _transapp;
        public BuspassprintformFacadeController(BuspassprintformInterface _inter)
        {
            _transapp = _inter;
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
        [Route("getdata/{id:int}")]
        public TransportApprovedDTO getdata(int id)
        {
            return _transapp.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("searchdetails")]
        public TransportApprovedDTO searchdetails([FromBody] TransportApprovedDTO data)
        {
            return _transapp.searchdetails(data);
        }
        [Route("showmodaldetails")]
        public Task<TransportApprovedDTO> showmodaldetails([FromBody] TransportApprovedDTO data)
        {
            return _transapp.showmodaldetails(data);
        }
        [Route("savelist")]
        public TransportApprovedDTO savelist([FromBody] TransportApprovedDTO data)
        {
            return _transapp.savelist(data);
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

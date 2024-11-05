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
    public class CLGTransportApproveFacadeController : Controller
    {
        public CLGTransportApproveInterface _transapp;
        public CLGTransportApproveFacadeController(CLGTransportApproveInterface _inter)
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
        [Route("getdata")]
        public CLGTransportApproveDTO getdata([FromBody] CLGTransportApproveDTO id)
        {
            return _transapp.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("searchdetails")]
        public CLGTransportApproveDTO searchdetails([FromBody] CLGTransportApproveDTO data)
        {
            return _transapp.searchdetails(data);
        }
        [Route("gridaconchange")]
        public CLGTransportApproveDTO gridaconchange([FromBody] CLGTransportApproveDTO data)
        {
            return _transapp.gridaconchange(data);
        }
        [Route("showmodaldetails")]
        public Task<CLGTransportApproveDTO> showmodaldetails([FromBody] CLGTransportApproveDTO data)
        {
            return _transapp.showmodaldetails(data);
        }
        [Route("savelist")]
        public CLGTransportApproveDTO savelist([FromBody] CLGTransportApproveDTO data)
        {
            return _transapp.savelist(data);
        }
        [Route("editapprove")]
        public CLGTransportApproveDTO editapprove([FromBody] CLGTransportApproveDTO data)
        {
            return _transapp.editapprove(data);
        }
        [Route("CancelRejection")]
        public CLGTransportApproveDTO CancelRejection([FromBody] CLGTransportApproveDTO data)
        {
            return _transapp.CancelRejection(data);
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

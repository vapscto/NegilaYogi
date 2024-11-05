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
    public class LocationFeeGroupMappingFacade : Controller
    {
        public LocationFeeGroupMappingInterface _areaint;

        public LocationFeeGroupMappingFacade(LocationFeeGroupMappingInterface areaz)
        {
            _areaint = areaz;
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
        public TR_Location_FeeGroup_MappingDTO getdata(int id)
        {

            return _areaint.getdata(id);
        }

        [Route("savedata")]
        public TR_Location_FeeGroup_MappingDTO savedata([FromBody]TR_Location_FeeGroup_MappingDTO data)
        {
            return _areaint.savedata(data);
        }
        [Route("geteditdata")]
        public TR_Location_FeeGroup_MappingDTO geteditdata([FromBody] TR_Location_FeeGroup_MappingDTO data)
        {
            return _areaint.geteditdata(data);
        }
        [Route("activedeactive")]
        public TR_Location_FeeGroup_MappingDTO activedeactive([FromBody] TR_Location_FeeGroup_MappingDTO data)
        {
            return _areaint.activedeactive(data);
        }


        [Route("savedataamount")]
        public TR_Location_AmountDTO savedataamount([FromBody]TR_Location_AmountDTO data)
        {
            return _areaint.savedataamount(data);
        }
        [Route("geteditdataamount")]
        public TR_Location_AmountDTO geteditdataamount([FromBody] TR_Location_AmountDTO data)
        {
            return _areaint.geteditdataamount(data);
        }
        [Route("activedeactiveamount")]
        public TR_Location_AmountDTO activedeactiveamount([FromBody] TR_Location_AmountDTO data)
        {
            return _areaint.activedeactiveamount(data);
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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

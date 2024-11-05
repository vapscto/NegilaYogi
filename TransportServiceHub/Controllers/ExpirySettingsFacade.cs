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
    public class ExpirySettingsFacade : Controller
    {
        public ExpirySettingsInterface _areaint;

        public ExpirySettingsFacade(ExpirySettingsInterface areaz)
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
        public ExpirySettingsDTO getdata(int id)
        {

            return _areaint.getdata(id);
        }

        [Route("savedata")]
        public ExpirySettingsDTO savedata([FromBody]ExpirySettingsDTO data)
        {
            return _areaint.savedata(data);
        }
        [Route("getdatadetails")]
        public ExpirySettingsDTO getdatadetails([FromBody]ExpirySettingsDTO data)
        {
            return _areaint.getdatadetails(data);
        }
        [Route("getsmsdetails")]
        public ExpirySettingsDTO getsmsdetails([FromBody]ExpirySettingsDTO data)
        {
            return _areaint.getsmsdetails(data);
        }
        [Route("jshsgetsmsdetails")]
        public ExpirySettingsDTO jshsgetsmsdetails([FromBody]ExpirySettingsDTO data)
        {
            return _areaint.jshsgetsmsdetails(data);
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

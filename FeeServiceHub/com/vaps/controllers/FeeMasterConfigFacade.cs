using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeMasterConfigFacade : Controller
    {
        public FeeMasterConfigInterface _feegrouppage;

        public FeeMasterConfigFacade(FeeMasterConfigInterface maspag)
        {
            _feegrouppage = maspag;
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
        [Route("getdetailsY")]
        public FeeMasterConfigurationDTO getdetailsY([FromBody] FeeMasterConfigurationDTO org)
        {
            return _feegrouppage.getdetailsY(org);
        }
        // POST api/values
        [HttpPost]
        public FeeMasterConfigurationDTO savedetail([FromBody] FeeMasterConfigurationDTO org)
        {
            return _feegrouppage.SaveconfigData(org);
        }


        [Route("savedetails/")]
        public FeeMasterConfigurationDTO savedetails([FromBody] FeeMasterConfigurationDTO org)
        {
            return _feegrouppage.SaveconfigData(org);
        }

        [Route("editdetails/")]
        public FeeMasterConfigurationDTO editdetails([FromBody] FeeMasterConfigurationDTO org)
        {
            return _feegrouppage.editdetails(org);
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
        // POST api/values
     
     
    }
}

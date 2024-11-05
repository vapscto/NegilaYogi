using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class MasterClgFeeConfigFacade : Controller
    {
        public MasterClgFeeConfigInterface _feegrouppage;

        public MasterClgFeeConfigFacade(MasterClgFeeConfigInterface maspag)
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
        public MasterClgFeeConfigDTO getdetailsY([FromBody] MasterClgFeeConfigDTO org)
        {
            return _feegrouppage.getdetailsY(org);
        }
        // POST api/values
        [HttpPost]      
        [Route("savedetails/")]
        public MasterClgFeeConfigDTO savedetails([FromBody] MasterClgFeeConfigDTO org)
        {
            return _feegrouppage.SaveconfigData(org);
        }

        [HttpPost]
        [Route("editdetails/")]
        public MasterClgFeeConfigDTO editdetails([FromBody] MasterClgFeeConfigDTO org)
        {
            return _feegrouppage.editdetails(org);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using FeeServiceHub.com.vaps.interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeMasterConcessionFacadeController : Controller
    {
        public FeeMasterConcessionInterface _feegrouppage;

        public FeeMasterConcessionFacadeController(FeeMasterConcessionInterface maspag)
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


        [Route("getdata")]
        public FeeMasterConcessionDTO getdata([FromBody]FeeMasterConcessionDTO id)
        {
            return _feegrouppage.getdata(id);
        }

        [Route("savedata")]
        public FeeMasterConcessionDTO savedata([FromBody] FeeMasterConcessionDTO data)
        {
            return _feegrouppage.savedata(data);
        }
        [Route("edit3")]
        public FeeMasterConcessionDTO edit3([FromBody] FeeMasterConcessionDTO data)
        {
            return _feegrouppage.edit3(data);
        }


        [Route("savedata2")]
        public FeeMasterConcessionDTO savedata2([FromBody] FeeMasterConcessionDTO data)
        {
            return _feegrouppage.savedata2(data);
        }



        [Route("activedeactive")]
        public FeeMasterConcessionDTO activedeactive([FromBody] FeeMasterConcessionDTO data)
        {
            return _feegrouppage.activedeactive(data);
        }


        [Route("deactive2")]
        public FeeMasterConcessionDTO deactive2([FromBody] FeeMasterConcessionDTO data)
        {
            return _feegrouppage.deactive2(data);
        }



        [Route("deactive3")]
        public FeeMasterConcessionDTO deactive3([FromBody] FeeMasterConcessionDTO data)
        {
            return _feegrouppage.deactive3(data);
        }


        [Route("editdata")]
        public FeeMasterConcessionDTO editdata([FromBody] FeeMasterConcessionDTO data)
        {
            return _feegrouppage.editdata(data);
        }

        [Route("gethead")]
        public FeeMasterConcessionDTO gethead([FromBody] FeeMasterConcessionDTO data)
        {
            return _feegrouppage.gethead(data);
        }

        [Route("savedata3")]
        public FeeMasterConcessionDTO savedata3([FromBody] FeeMasterConcessionDTO data)
        {
            return _feegrouppage.savedata3(data);
        }

        [Route("edit2")]
        public FeeMasterConcessionDTO edit2([FromBody] FeeMasterConcessionDTO data)
        {
            return _feegrouppage.edit2(data);
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

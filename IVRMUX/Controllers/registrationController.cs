using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    public class registrationController : Controller
    {
        registrationdelegate registr = new registrationdelegate();
        //// GET: api/values
        [HttpGet]
       public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        ////// GET api/values/5
        ////[HttpGet("{id}")]
        ////public string Get(int id)
        ////{
        ////    return "value";
        ////}

        //// POST api/values
        //[HttpPost]
        //public object registration([FromBody]regis reg)
        //{
        //    pre.regdata(reg);
        //    return reg;
        //}

        ////// PUT api/values/5
        ////[HttpPut("{id}")]
        ////public void Put(int id, [FromBody]string value)
        ////{
        ////}

        ////// DELETE api/values/5
        ////[HttpDelete("{id}")]
        ////public void Delete(int id)
        ////{
        ////}

        [HttpPost]
       // public IActionResult Post([FromBody] regis reg)
            public regis regis ([FromBody] regis reg)
        {
            var original = "";
            registr.regdata(reg);
            //reg.status = "sucess";
            return reg;
            // return reg;
        }
    }
}

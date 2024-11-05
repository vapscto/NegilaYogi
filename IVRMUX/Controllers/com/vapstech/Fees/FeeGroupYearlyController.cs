using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{

    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeGroupYearlyController : Controller
    {
        FeeGroupYearlyDelegate FGD1 = new FeeGroupYearlyDelegate();
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
        public FeeYearlyGroupDTO savedetail([FromBody] FeeYearlyGroupDTO GrouppageY)
        {

            int pageid = 0;
            if (HttpContext.Session.GetString("pageid") != null)
            {
                pageid = Convert.ToInt32(HttpContext.Session.GetString("pageid"));//Get
            }

            GrouppageY.FYG_Id = pageid;
            HttpContext.Session.Remove("pageid");
            return FGD1.savedetail(GrouppageY);
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

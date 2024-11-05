using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CategoryWiseTotalStrengthController : Controller
    {
        // GET: api/values
        CategoryWiseTotalStrengthDTO dto = new CategoryWiseTotalStrengthDTO();
        CategoryWiseTotalStrengthDelegate FGD = new CategoryWiseTotalStrengthDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CategoryWiseTotalStrengthDTO Get(int id)
        {
            CategoryWiseTotalStrengthDTO data = new CategoryWiseTotalStrengthDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGD.getdetails(data);


        }

        // POST api/values

        [Route("Getreportdetails")]
        public CategoryWiseTotalStrengthDTO Getreportdetails([FromBody] CategoryWiseTotalStrengthDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;
            return FGD.Getreportdetails(MMD);
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

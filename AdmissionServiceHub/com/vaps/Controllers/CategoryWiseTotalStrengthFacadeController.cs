using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using AdmissionServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CategoryWiseTotalStrengthFacadeController : Controller
    {
        public CategoryWiseTotalStrengthInterface _feegrouppagee;
        // GET: api/values

        public CategoryWiseTotalStrengthFacadeController(CategoryWiseTotalStrengthInterface maspag)
        {
            _feegrouppagee = maspag;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("getdetails")]
        public CategoryWiseTotalStrengthDTO getorgdet([FromBody] CategoryWiseTotalStrengthDTO data)
        {
            return _feegrouppagee.getdetails(data);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("Getreportdetails")]
        public Task<CategoryWiseTotalStrengthDTO> Getreportdetails([FromBody] CategoryWiseTotalStrengthDTO data)
        {
            return _feegrouppagee.Getreportdetails(data);
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

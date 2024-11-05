using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ClasswisestudentdetailsFacadeController : Controller
    {


        public ClasswisestudentdetailsInterface _feegrouppagee;
        // GET: api/values

        public ClasswisestudentdetailsFacadeController(ClasswisestudentdetailsInterface maspag)
        {
            _feegrouppagee = maspag;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public ClasswisestudentdetailsDTO getdetails([FromBody]ClasswisestudentdetailsDTO data)
        {
            return _feegrouppagee.getdetails(data);
        }

        [Route("Getreportdetails")]
        public Task<ClasswisestudentdetailsDTO> Getreportdetails([FromBody] ClasswisestudentdetailsDTO data)
        {
            return _feegrouppagee.Getreportdetails(data);
        }
        [Route("getsection")]
        public ClasswisestudentdetailsDTO getsection([FromBody] ClasswisestudentdetailsDTO data)
        {
            return _feegrouppagee.getsection(data);
        }
        [Route("fetchclassbyYearId")]
        public ClasswisestudentdetailsDTO fetchclassbyYearId([FromBody] ClasswisestudentdetailsDTO data)
        {
            return _feegrouppagee.fetchclassbyYearId(data);
        }
        

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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

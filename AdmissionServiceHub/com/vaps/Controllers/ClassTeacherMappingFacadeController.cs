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
    public class ClassTeacherMappingFacadeController : Controller
    {
      public  ClassTeacherMappingInterface _interface;
        public ClassTeacherMappingFacadeController(ClassTeacherMappingInterface enqui)
        {
            _interface = enqui;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails/{id:int}")]
        public ClassTeacherMappingDTO getdetails(int id)
        {
            return _interface.getdetails(id);
        }

        [Route("save")]
        public ClassTeacherMappingDTO save([FromBody]ClassTeacherMappingDTO data)
        {
            return _interface.save(data);
        }

        [Route("GetSelectedRowDetails")]
        public ClassTeacherMappingDTO GetSelectedRowDetails([FromBody] ClassTeacherMappingDTO data)
        {
            return _interface.GetSelectedRowDetails(data);
        }
        [Route("onchangestaff2")]
        public ClassTeacherMappingDTO onchangestaff2([FromBody] ClassTeacherMappingDTO data)
        {
            return _interface.onchangestaff2(data);
        }

        [Route("onchangestaff1")]
        public ClassTeacherMappingDTO onchangestaff1([FromBody] ClassTeacherMappingDTO data)
        {
            return _interface.onchangestaff1(data);
        }
        [Route("exchangesave")]
        public ClassTeacherMappingDTO exchangesave([FromBody] ClassTeacherMappingDTO data)
        {
            return _interface.exchangesave(data);
        }
        [Route("deleterecord")]
        public ClassTeacherMappingDTO deleterecord([FromBody] ClassTeacherMappingDTO data)
        {
            return _interface.deleterecord(data);
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

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
    public class StudentSearchFacade : Controller
    {
        StudentSearchInterface _int;
        public StudentSearchFacade(StudentSearchInterface stu)
        {
            _int = stu;
        }


        // GET api/values/5
        [HttpGet]
        public void Get(int id)
        {
          
        }

        // POST api/values
        [HttpPost]
        public Adm_M_StudentDTO Post([FromBody]Adm_M_StudentDTO value)
        {
            return _int.getData(value);
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

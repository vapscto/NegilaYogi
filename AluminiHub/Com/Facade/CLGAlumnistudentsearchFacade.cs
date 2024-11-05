using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumniHub.Com.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlumniHub.Com.Facade
{
    [Route("api/[controller]")]
    public class CLGAlumnistudentsearchFacade : Controller 
    {

        CLGAlumnistudentsearchInterface _int;
        public CLGAlumnistudentsearchFacade(CLGAlumnistudentsearchInterface stu)
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
        public CLGAlumniStudentDTO Post([FromBody]CLGAlumniStudentDTO value)
        {
            return _int.getData(value);
        }
        [Route("getsemdata")]
        public CLGAlumniStudentDTO getsemdata([FromBody]CLGAlumniStudentDTO value)
        {
            return _int.getsemdata(value);
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

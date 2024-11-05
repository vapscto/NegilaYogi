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
    public class MasterClassHeldFacade : Controller
    {
        public MasterClassHeldInterface _mch;
        public MasterClassHeldFacade(MasterClassHeldInterface mch)
        {
            _mch = mch;
        }
        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
       
        [Route("getDetails")]
        public MasterClassHeldDTO Get([FromBody]MasterClassHeldDTO dto )
        {
            return _mch.getAllDetails(dto);
        }

        // POST api/values
        [HttpPost]
        public MasterClassHeldDTO Post([FromBody]MasterClassHeldDTO mstclassheld)
        {
            return _mch.saveData(mstclassheld);
        }
        [Route("getNoOfClassHeld")]
        public MasterClassHeldDTO getNoOfClassHeld([FromBody] MasterClassHeldDTO dto)
        {
            return _mch.getNoOfClassHeld(dto);
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

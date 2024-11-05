using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class AdditionalFieldFacadeController : Controller
    {
        //public AdditionalFieldInterface _IAdditionalFied;
        public AdditionalFieldInterface _catry;
        public AdditionalFieldFacadeController(AdditionalFieldInterface enqui)
        {
            _catry = enqui;
        }
      

        // GET api/values/5
       [HttpGet]
       [Route("getbasicdata/{id:int}")]
        public AdditionalFieldDTO getBasicData(int id)
        {
            return _catry.getBasicData(id);
        }

        [Route("editdata/{id:int}")]
        public AdditionalFieldDTO editData(int id)
        {
            return _catry.editData(id);
        }

        [HttpPost]
        // POST api/values
        public AdditionalFieldDTO Save([FromBody] AdditionalFieldDTO cdto)
        {
            return _catry.Save(cdto);
        }
        [Route("deactivate/{id:int}")]
        public void deactivate(int id)
        {
            // id = 12;
            _catry.deactivate(id);
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

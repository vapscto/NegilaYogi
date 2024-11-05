using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.Controllers
{
    [Route("api/[controller]")]
    public class DOBcertificateFacade : Controller
    {
        public DOBcertificateInterface _ads;

        public DOBcertificateFacade(DOBcertificateInterface adstu)
        {
            _ads = adstu;
        }
        // GET: api/values
        [HttpGet]
        [Route("getdata/{id:int}")]
        public Adm_M_StudentDTO getinitialdata(int id)
        {
            Adm_M_StudentDTO stud = new Adm_M_StudentDTO();
            stud.MI_Id = id;
            return _ads.getdetails(stud);
        }

        [HttpPost]
        [Route("getstudbyclass")]
        public Adm_M_StudentDTO getStudDatabyclass([FromBody] Adm_M_StudentDTO data)
        {
            return _ads.getStudDatabyclass(data);
        }


        //[HttpGet]
        //[Route("getStudData")]
        //public Task<Adm_M_StudentDTO> getStudData([FromBody] Adm_M_StudentDTO studData)
        //{
        //    return _ads.getStudDetails(studData);
        //}

        [HttpPost]
        [Route("getStudData")]
        public Task<Adm_M_StudentDTO> Post([FromBody] Adm_M_StudentDTO studData)
        {
            return _ads.getStudDetails(studData);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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

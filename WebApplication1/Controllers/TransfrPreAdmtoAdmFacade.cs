using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;
using PreadmissionDTOs.com.vaps.admission;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class TransfrPreAdmtoAdmFacade : Controller
    {
        public TransfrPreAdmtoAdmInterface _acd;
        public TransfrPreAdmtoAdmFacade(TransfrPreAdmtoAdmInterface acdm)
        {
            _acd = acdm;
        }
        // GET: api/values
        [HttpGet]
        [Route("TrnfPreadmtoAdm/{id:int}")]
        public Adm_M_StudentDTO Get(int id)
        {
            return _acd.getAcademicdata(id);
        }

        // POST api/values
        [HttpPost]
        [Route("searchdata")]
        public Adm_M_StudentDTO searchdat([FromBody] Adm_M_StudentDTO data)
        {
            return _acd.getserdata(data);
        }

        [Route("exporttoadmission")]
        public async Task<Adm_M_StudentDTO> expoadm([FromBody] Adm_M_StudentDTO data)
        {
            return await _acd.expoadmi(data);
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

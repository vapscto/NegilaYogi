using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class DOBcertificateController : Controller
    {
        DOBcertificateDelegate adsd = new DOBcertificateDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getdata/{id:int}")]
        public Adm_M_StudentDTO getinitialdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getdetails(id);
        }

        [HttpPost]
        [Route("getstudbyclass")]
        public Adm_M_StudentDTO getStudDatabyclass([FromBody] Adm_M_StudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return adsd.getStudDatabyclass(data);
        }


        [Route("Studdetails")]

        public Adm_M_StudentDTO getStudData([FromBody] Adm_M_StudentDTO stuDTO)
        {
            return adsd.GetStudDataById(stuDTO);
        }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class TransfrPreAdmtoAdmController : Controller
    {
        TransfrPreAdmtoAdmDelegate tra = new TransfrPreAdmtoAdmDelegate();
        // GET: api/values
        [HttpGet]
        [Route("TrnfPreadmtoAdm/{id:int}")]
        public Adm_M_StudentDTO Get(int id)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = mid;
            return tra.getAcademicdata(id);
        }

        // POST api/values
        [HttpPost]
        [Route("searchdata")]
        public Adm_M_StudentDTO searchdat([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            return tra.getserdata(data);
        }

        [Route("exporttoadmission")]
        public Adm_M_StudentDTO expoadm([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return tra.expoadmi(data);
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

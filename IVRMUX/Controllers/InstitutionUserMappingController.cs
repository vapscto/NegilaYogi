using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers
{
    [Route("api/[controller]")]
    public class InstitutionUserMappingController : Controller
    {
        InstitutionUserMappingDelegate _delg = new InstitutionUserMappingDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata/{id:int}")]
        public InstitutionUserMappingDTO loaddata(int id)
        {
            InstitutionUserMappingDTO data = new InstitutionUserMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.loaddata(data);
        }

        [Route("onchangeinst")]
        public InstitutionUserMappingDTO onchangeinst([FromBody] InstitutionUserMappingDTO data)
        {
            return _delg.onchangeinst(data);
        }

        [Route("savedetails")]
        public InstitutionUserMappingDTO savedetails([FromBody] InstitutionUserMappingDTO data)
        {
            return _delg.savedetails(data);
        }

        [Route("viewdetails")]
        public InstitutionUserMappingDTO viewdetails([FromBody] InstitutionUserMappingDTO data)
        {
            return _delg.viewdetails(data);
        }


        [Route("savepaymentremarks")]
        public InstitutionUserMappingDTO savepaymentremarks([FromBody]InstitutionUserMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            HttpContext.Session.SetInt32(data.paymentsubscriptiontype, 1);
            return _delg.savepaymentremarks(data);
        }


        [Route("setsmscreditsession")]
        public InstitutionUserMappingDTO setsmscreditsession([FromBody]InstitutionUserMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            HttpContext.Session.SetInt32("smscreditalert", 1);
            data.smscreditalert= Convert.ToInt64(HttpContext.Session.GetInt32("smscreditalert"));
            return data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admission.com.vapstech.delegates;
using Microsoft.AspNetCore.Cors;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Admission.com.vapstech.controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AdditionalFieldsController : Controller
    {
        // GET: api/values
        AdditionalFieldsDelegate extfield_delobj = new AdditionalFieldsDelegate();
        AdditionalFieldsDelegate addi_deact = new AdditionalFieldsDelegate();

        // POST api/values
        [Route("getbasicdata/{id:int}")]
        public AdditionalFieldDTO getBasicData(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return extfield_delobj.getBasicData(id);
        }

        [Route("editdata/{id:int}")]
        public AdditionalFieldDTO EditData(int id)
        {
            return extfield_delobj.EditData(id);
        }
        // POST api/values
        [HttpPost]
        public AdditionalFieldDTO Save([FromBody] AdditionalFieldDTO adddto)
        {
            adddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return extfield_delobj.savedetails(adddto);
        }

        [Route("deactivate/{id:int}")]
        public void deactvate(int id)
        {
            addi_deact.deactivate(id);
        }
    }
}

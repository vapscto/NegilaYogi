using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class ProgramMasterController : Controller
    {
        ProgramMasterDelegate delobj = new ProgramMasterDelegate();

        [Route("loadgrid/{id:int}")]
        public ProgramMasterDTO getDetails(int id)
        {
            ProgramMasterDTO data = new ProgramMasterDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.getDetails(data);
        }

        [Route("saveRecord")]
        public ProgramMasterDTO saveRecord([FromBody]ProgramMasterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.save(data);
        }
        [Route("Edit/{id:int}")]
        public ProgramMasterDTO Edit(int id)
        {
            return delobj.EditDetails(id);
        }
        [Route("deactivate")]
        public ProgramMasterDTO deactivate([FromBody] ProgramMasterDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.deactivate(d);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VidyaBharathi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VidyaBharathi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VidyaBharathi
{
    [Route("api/[controller]")]
    public class VBSC_Master_SportsCCName_UOMController : Controller
    {
        VBSC_Master_SportsCCName_UOMDelegate delegat = new VBSC_Master_SportsCCName_UOMDelegate();

        [Route("getDetails/{id:int}")]
        public VBSC_Master_SportsCCName_UOMDTO getDetails(int id)
        {
            VBSC_Master_SportsCCName_UOMDTO dto = new VBSC_Master_SportsCCName_UOMDTO();
           //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.getDetails(dto);
        }

        [Route("saveRecord")]
        public VBSC_Master_SportsCCName_UOMDTO saveRecord([FromBody]VBSC_Master_SportsCCName_UOMDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.save(data);
        }

        [Route("Edit/{id:int}")]
        public VBSC_Master_SportsCCName_UOMDTO Edit(int id)
        {
            return delegat.EditDetails(id);
        }

        [Route("deactivate")]
        public VBSC_Master_SportsCCName_UOMDTO deactivate([FromBody] VBSC_Master_SportsCCName_UOMDTO d)
        {
            //d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.deactivate(d);
        }
    }
}


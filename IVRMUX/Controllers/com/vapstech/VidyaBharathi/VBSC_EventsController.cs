using IVRMUX.Delegates.com.vapstech.VidyaBharathi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.VidyaBharathi
{
    [Route("api/[controller]")]
    public class VBSC_EventsController : Controller
    {
        VBSC_EventsDelegate _delegate = new VBSC_EventsDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public VBSC_EventsDTO getloaddata(int id)
        {
            VBSC_EventsDTO data = new VBSC_EventsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public VBSC_EventsDTO savedetails([FromBody] VBSC_EventsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public VBSC_EventsDTO deactive([FromBody] VBSC_EventsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }

      

    }
}

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
    public class VBSC_Master_EventsController : Controller
    {
        VBSC_Master_EventsDelegate _delegate = new VBSC_Master_EventsDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public VBSC_Master_EventsDTO getloaddata(int id)
        {
            VBSC_Master_EventsDTO data = new VBSC_Master_EventsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public VBSC_Master_EventsDTO savedetails([FromBody] VBSC_Master_EventsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public VBSC_Master_EventsDTO deactive([FromBody] VBSC_Master_EventsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }


    }
}

using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidyaBharathiServiceHub.com.vaps.Interfaces;

namespace VidyaBharathiServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VBSC_EventsFacadeController : Controller
    {
        // GET: api/values
        VBSC_EventsInterface _cms;
        public VBSC_EventsFacadeController(VBSC_EventsInterface cms)
        {
            _cms = cms;
        }

        [Route("getloaddata")]
        public VBSC_EventsDTO getloaddata([FromBody] VBSC_EventsDTO data)
        {
            return _cms.getloaddata(data);
        }
        [HttpPost]
        [Route("savedetails")]
        public VBSC_EventsDTO savedetails([FromBody] VBSC_EventsDTO data)
        {
            return _cms.savedetails(data);
        }

        [Route("deactive")]
        public VBSC_EventsDTO deactive([FromBody] VBSC_EventsDTO data)
        {
            return _cms.deactive(data);
        }
        

    }

}

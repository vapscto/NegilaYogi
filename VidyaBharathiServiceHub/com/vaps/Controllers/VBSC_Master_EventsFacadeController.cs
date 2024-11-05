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
    public class VBSC_Master_EventsFacadeController : Controller
    {
        // GET: api/values
        VBSC_Master_EventsInterface _cms;
        public VBSC_Master_EventsFacadeController(VBSC_Master_EventsInterface cms)
        {
            _cms = cms;
        }

        [Route("getloaddata")]
        public VBSC_Master_EventsDTO getloaddata([FromBody] VBSC_Master_EventsDTO data)
        {
            return _cms.getloaddata(data);
        }
        [HttpPost]
        [Route("savedetails")]
        public VBSC_Master_EventsDTO savedetails([FromBody] VBSC_Master_EventsDTO data)
        {
            return _cms.savedetails(data);
        }

        [Route("deactive")]
        public VBSC_Master_EventsDTO deactive([FromBody] VBSC_Master_EventsDTO data)
        {
            return _cms.deactive(data);
        }



    }

}

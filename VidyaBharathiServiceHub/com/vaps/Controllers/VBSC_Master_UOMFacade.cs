using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using VidyaBharathiServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VidyaBharathiServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VBSC_Master_UOMFacade : Controller
    {
      
        public VBSC_Master_UOMInterface _cms;

        public VBSC_Master_UOMFacade(VBSC_Master_UOMInterface cmsdept)
        {
            _cms = cmsdept;
        }

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public VBSC_Master_UOMDTO loaddata(int id)
        {
            return _cms.loaddata(id);
            // return _cms.loaddata(id);
        }

        [HttpPost]
        [Route("savedata")]
        public VBSC_Master_UOMDTO savedetails([FromBody]VBSC_Master_UOMDTO data)
        {
            return _cms.savedetails(data);
        }

        [Route("deactive")]
        public VBSC_Master_UOMDTO deactive([FromBody] VBSC_Master_UOMDTO data)
        {
            return _cms.deactive(data);
        }

        //competition level

        [HttpGet]
        [Route("getloaddatalevel/{id:int}")]
        public VBSC_Master_UOMDTO getloaddatalevel(int id)
        {
            return _cms.getloaddatalevel(id);
            // return _cms.loaddata(id);
        }

        
        [HttpPost]
        [Route("savedatalevel")]
        public VBSC_Master_UOMDTO savedetailslevel([FromBody]VBSC_Master_UOMDTO data)
        {
            return _cms.savedetailslevel(data);
        }

        [Route("deactivelevel")]
        public VBSC_Master_UOMDTO deactivelevel([FromBody] VBSC_Master_UOMDTO data)
        {
            return _cms.deactivelevel(data);
        }

    }
}

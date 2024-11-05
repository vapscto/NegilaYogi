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
    public class VBSC_Master_SportsCCGroupNameFacadeController : Controller
    {
        // GET: api/values
        VBSC_Master_SportsCCGroupNameInterface _cms;
        public VBSC_Master_SportsCCGroupNameFacadeController(VBSC_Master_SportsCCGroupNameInterface cms)
        {
            _cms = cms;
        }

        [Route("getloaddata")]
        public VBSC_Master_SportsCCGroupNameDTO getloaddata([FromBody] VBSC_Master_SportsCCGroupNameDTO data)
        {
            return _cms.getloaddata(data);
        }
        
        [Route("savedetails")]
        public VBSC_Master_SportsCCGroupNameDTO savedetails([FromBody] VBSC_Master_SportsCCGroupNameDTO data)
        {
            return _cms.savedetails(data);
        }

        [Route("deactive")]
        public VBSC_Master_SportsCCGroupNameDTO deactive([FromBody] VBSC_Master_SportsCCGroupNameDTO data)
        {
            return _cms.deactive(data);
        }



    }
}

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
    public class VBSC_Master_SportsCCNameFacadeController : Controller
    {
        // GET: api/values
        VBSC_Master_SportsCCNameInterface _cms;
        public VBSC_Master_SportsCCNameFacadeController(VBSC_Master_SportsCCNameInterface cms)
        {
            _cms = cms;
        }

        [Route("getloaddata")]
        public VBSC_Master_SportsCCNameDTO getloaddata([FromBody] VBSC_Master_SportsCCNameDTO data)
        {
            return _cms.getloaddata(data);
        }

        [Route("getInstitute")]
        public VBSC_Master_SportsCCNameDTO getInstitute([FromBody] VBSC_Master_SportsCCNameDTO data)
        {
            return _cms.getInstitute(data);
        }

        [Route("savedetails")]
        public VBSC_Master_SportsCCNameDTO savedetails([FromBody] VBSC_Master_SportsCCNameDTO data)
        {
            return _cms.savedetails(data);
        }

        [Route("deactive")]
        public VBSC_Master_SportsCCNameDTO deactive([FromBody] VBSC_Master_SportsCCNameDTO data)
        {
            return _cms.deactive(data);
        }
        
    }
}

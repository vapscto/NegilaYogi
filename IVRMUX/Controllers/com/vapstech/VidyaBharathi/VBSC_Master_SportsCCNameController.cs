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
    public class VBSC_Master_SportsCCNameController : Controller
    {
        VBSC_Master_SportsCCNameDelegate _delegate = new VBSC_Master_SportsCCNameDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public VBSC_Master_SportsCCNameDTO getloaddata(int id)
        {
            VBSC_Master_SportsCCNameDTO data = new VBSC_Master_SportsCCNameDTO();
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }


        [HttpPost]
        [Route("getInstitute")]
        public VBSC_Master_SportsCCNameDTO getInstitute([FromBody] VBSC_Master_SportsCCNameDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getInstitute(data);
        }

    
        [Route("savedetails")]
        public VBSC_Master_SportsCCNameDTO savedetails([FromBody] VBSC_Master_SportsCCNameDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public VBSC_Master_SportsCCNameDTO deactive([FromBody] VBSC_Master_SportsCCNameDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
    }
}

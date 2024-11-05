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
    public class VBSC_Events_Category : Controller
    {
        VBSC_Events_CategoryDelegate cms = new VBSC_Events_CategoryDelegate();


        [HttpGet]
        [Route("loaddata/{id:int}")]
        public VBSC_Events_CategoryDTO loaddata(int id)
        {
            VBSC_Events_CategoryDTO dto = new VBSC_Events_CategoryDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.loaddata(dto);
        }
        [HttpPost]
        [Route("savedata")]
        public VBSC_Events_CategoryDTO savedata([FromBody]VBSC_Events_CategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }

        [Route("deactive")]
        public VBSC_Events_CategoryDTO deactive([FromBody] VBSC_Events_CategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.deactive(data);
        }


    }
}

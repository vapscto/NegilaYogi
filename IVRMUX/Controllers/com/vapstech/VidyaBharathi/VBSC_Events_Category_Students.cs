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
    public class VBSC_Events_Category_Students : Controller
    {
        VBSC_Events_Category_StudentsDelegate cms = new VBSC_Events_Category_StudentsDelegate();


        [HttpGet]
        [Route("loaddata/{id:int}")]
        public VBSC_Events_Category_StudentsDTO loaddata(int id)
        {
            VBSC_Events_Category_StudentsDTO dto = new VBSC_Events_Category_StudentsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.loaddata(dto);
        }
        [HttpPost]
        [Route("savedata")]
        public VBSC_Events_Category_StudentsDTO savedata([FromBody]VBSC_Events_Category_StudentsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }

        [Route("deactive")]
        public VBSC_Events_Category_StudentsDTO deactive([FromBody] VBSC_Events_Category_StudentsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.deactive(data);
        }


    }
}

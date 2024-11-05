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
    public class VBadmin : Controller
    {
        VBadminDelegate cms = new VBadminDelegate();


        [HttpGet]
        [Route("LoadData/{id:int}")]
        public VBadminDTO LoadData(int id)
        {
            VBadminDTO dto = new VBadminDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.LoadData(dto);
        }
        [HttpGet]
        [Route("ViewCOEDetails/{id:int}")]
        public VBadminDTO ViewCOEDetails(int id)
        {
            VBadminDTO dto = new VBadminDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.ViewCOEDetails(dto);
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Placement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Placement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Placement
{
    [Route("api/[controller]")]
    public class PL_Master_Company_ContactController : Controller
    {
        PL_Master_Company_ContactDelgate cms = new PL_Master_Company_ContactDelgate();

        //[HttpGet]
        //[Route("loaddata/{id:int}")]
        //public PL_Master_Company_ContactDTO loaddata(int id)
        //{
        //    id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                    
        //    return cms.loaddata(id);

        //}

        [Route("loaddata/{id:int}")]
        public PL_Master_Company_ContactDTO loaddata(int id)
        {
            PL_Master_Company_ContactDTO data = new PL_Master_Company_ContactDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.loaddata(data);
        }

        [Route("savedata")]
        public PL_Master_Company_ContactDTO savedata([FromBody] PL_Master_Company_ContactDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }
        [Route("deactive")]
        public PL_Master_Company_ContactDTO deactive([FromBody] PL_Master_Company_ContactDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.deactive(data);
        }
    }
}

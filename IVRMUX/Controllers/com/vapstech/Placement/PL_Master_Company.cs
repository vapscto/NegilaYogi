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
    public class PL_Master_Company : Controller
    {
        PL_Master_CompanyDelgate cms = new PL_Master_CompanyDelgate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public PL_Master_CompanyDTO loaddata(int id)
        {
            PL_Master_CompanyDTO data = new PL_Master_CompanyDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.loaddata(data);
        }

        [Route("savedata")]
        public PL_Master_CompanyDTO savedata([FromBody] PL_Master_CompanyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }
        [Route("deactive")]
        public PL_Master_CompanyDTO deactive([FromBody] PL_Master_CompanyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.deactive(data);
        }
    }
}

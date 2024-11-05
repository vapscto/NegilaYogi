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
    public class PL_CI_Schedule_CompanyController : Controller
    {
        PL_CI_Schedule_CompanyDelgate cms = new PL_CI_Schedule_CompanyDelgate();

        //[HttpGet]
        //[Route("loaddata/{id:int}")]
        //public PL_CI_Schedule_CompanyDTO loaddata(int id)
        //{
        //    id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                    
        //    return cms.loaddata(id);

        //}


        [Route("loaddata/{id:int}")]
        public PL_CI_Schedule_CompanyDTO loaddata(int id)
        {
            PL_CI_Schedule_CompanyDTO data = new PL_CI_Schedule_CompanyDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.loaddata(data);
        }

        [Route("savedata")]
        public PL_CI_Schedule_CompanyDTO savedata([FromBody] PL_CI_Schedule_CompanyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }
        [Route("deactive")]
        public PL_CI_Schedule_CompanyDTO deactive([FromBody] PL_CI_Schedule_CompanyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.deactive(data);
        }

        [Route("editdetails")]        public PL_CI_Schedule_CompanyDTO editdetails([FromBody] PL_CI_Schedule_CompanyDTO data)        {
            //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.editdetails(data);        }
    }
}

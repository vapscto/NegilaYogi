using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Portals.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Portals.Principal
{
    [Route("api/[controller]")]
    public class Clg_ClassDetailsController : Controller
    {
        Clg_ClassDetailsDelegate del = new Clg_ClassDetailsDelegate();
        [Route("loaddata/{id:int}")]
        public Clg_ClassDetails_DTO loaddata(int id)
        {
            Clg_ClassDetails_DTO data = new Clg_ClassDetails_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("getcourse")]
        public Clg_ClassDetails_DTO getcourse([FromBody]Clg_ClassDetails_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcourse(data);
        }
        [Route("report")]
        public Clg_ClassDetails_DTO report([FromBody]Clg_ClassDetails_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.report(data);
        }
    }
}

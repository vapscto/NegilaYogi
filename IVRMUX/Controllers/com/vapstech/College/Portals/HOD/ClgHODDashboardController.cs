using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgHODDashboardController : Controller
    {
        ClgHODDashboardDelegate clgdelegate = new ClgHODDashboardDelegate();

        [HttpGet]
        [Route("Getdetails")]      
        public ClgStudentDashboardDTO Getdetails(ClgStudentDashboardDTO data)
        {     
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));          
            data.AMCST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
         
            return clgdelegate.Getdetails(data);
        }



        [Route("savedetail")]
        public ClgStudentDashboardDTO save([FromBody] ClgStudentDashboardDTO lv)
        {
            //HOD_DTO lv = new HOD_DTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return clgdelegate.save(lv);
        }

        [Route("mappHOD")]
        public ClgStudentDashboardDTO mappHOD([FromBody] ClgStudentDashboardDTO lv)
        {
            
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.mappHOD(lv);
        }
        [Route("updateHOD/{id:int}")]
        public ClgStudentDashboardDTO updateHOD(int id)
        {
            ClgStudentDashboardDTO lv = new ClgStudentDashboardDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            lv.IHOD_Id = id;
            return clgdelegate.updateHOD(lv);
        }

        [Route("deactiveY")]
        public ClgStudentDashboardDTO deactiveY([FromBody] ClgStudentDashboardDTO lv)
        {

            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.deactiveY(lv);
        }



    }
}

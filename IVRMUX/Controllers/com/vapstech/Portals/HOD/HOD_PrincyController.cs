using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.FrontOffice;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.HOD;
using PreadmissionDTOs.com.vaps.Portals.HOD;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.HOD
{
    [Route("api/[controller]")]
    public class HOD_PrincyController : Controller
    {
        HOD_PrincyDelegate lcd = new HOD_PrincyDelegate();

        [Route("getalldetails/{id:int}")]
        public HOD_DTO getalldetails(int id)
        {
            HOD_DTO lv = new HOD_DTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getalldetails(lv);
        }
        [Route("savedetail")]
        public HOD_DTO save([FromBody] HOD_DTO lv)
        {
            //HOD_DTO lv = new HOD_DTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.save(lv);
        }

        [Route("mappHOD")]
        public HOD_DTO mappHOD([FromBody] HOD_DTO lv)
        {
            //HOD_DTO lv = new HOD_DTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.mappHOD(lv);
        }
        [Route("updateHOD/{id:int}")]
        public HOD_DTO updateHOD(int id)
        {
            HOD_DTO lv = new HOD_DTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            lv.IHOD_Id = id;
            return lcd.updateHOD(lv);
        }

        [Route("deactiveY")]
        public HOD_DTO deactiveY([FromBody] HOD_DTO lv)
        {

            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return lcd.deactiveY(lv);
        }
    }
}

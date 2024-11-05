using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.ClubManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.ClubManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.ClubManagement
{
    [Route("api/[controller]")]
    public class CMS_Master_MemberBlocked : Controller
    {
        CMS_Master_MemberBlockedDelegate cms = new CMS_Master_MemberBlockedDelegate();
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_Master_MemberBlockedDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.loaddata(id);

        }
        //CMS_Master_MemberBlockedDTO
        [HttpPost]
        [Route("savedetail1")]
        public CMS_Master_MemberBlockedDTO savedetail1([FromBody] CMS_Master_MemberBlockedDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return cms.savedetail1(categorypage);
        }
        //deactive
        [Route("deactive")]
        public CMS_Master_MemberBlockedDTO deactive([FromBody] CMS_Master_MemberBlockedDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return cms.deactive(categorypage);
        }
    }
}

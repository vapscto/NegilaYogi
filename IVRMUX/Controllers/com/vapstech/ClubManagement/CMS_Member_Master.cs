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

    public class CMS_Member_Master : Controller
    {
        CMS_Master_MemberDelegate cms = new CMS_Master_MemberDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_MastermemberDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.loaddata(id);

        }
        [HttpPost]
        [Route("savedetail1")]
        public CMS_MastermemberDTO savedetail1([FromBody] CMS_MastermemberDTO categorypage)
        {
            
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return cms.savedetail1(categorypage);
        }
        [HttpPost]
        [Route("editmember")]
        public CMS_MastermemberDTO editmember([FromBody] CMS_MastermemberDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.editmember(categorypage);
        }
        [Route("savedetail2")]
        public CMS_Master_Member_QualificationDTO savedetail2([FromBody] CMS_Master_Member_QualificationDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return cms.savedetail2(categorypage);
        }
        //savedetail3
        [Route("savedetail3")]
        public CMS_Master_Member_ExperienceDTO savedetail3([FromBody] CMS_Master_Member_ExperienceDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return cms.savedetail3(categorypage);
        }
        //savedetail5
        [Route("savedetail5")]
        public CMS_Master_MemberMobileNoDTO savedetail5([FromBody] CMS_Master_MemberMobileNoDTO categorypage)
        {         
            categorypage.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return cms.savedetail5(categorypage);
        }
        //savedetail6
        [Route("savedetail6")]
        public CMS_Master_Member_EmailDTO savedetail6([FromBody] CMS_Master_Member_EmailDTO categorypage)
        {
            categorypage.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return cms.savedetail6(categorypage);
        }
        [Route("savedetail7")]
        public CMS_MasterMember_DocumentsDTO savedetail7([FromBody] CMS_MasterMember_DocumentsDTO categorypage)
        {
            categorypage.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return cms.savedetail7(categorypage);
        }
        //deactive
        [Route("deactive")]
        public CMS_MastermemberDTO deactive([FromBody] CMS_MastermemberDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return cms.deactive(categorypage);
        }
        // 
    }
}

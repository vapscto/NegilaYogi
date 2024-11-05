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
   
    public class CMS_MemberCategory : Controller
    {
        CMS_MemberCategoryDelegate cms = new CMS_MemberCategoryDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        
        public CMS_MemberCategoryDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //CMS_MasterDepartmentDTO data = new CMS_MasterDepartmentDTO();
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return cms.loaddata(id);

        }
        [HttpPost]
        [Route("savedata")]
        public CMS_MemberCategoryDTO savedata([FromBody]CMS_MemberCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_MemberCategoryDTO deactive([FromBody]CMS_MemberCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.deactive(data);
        }
        [Route("edit")]
        public CMS_MemberCategoryDTO edit([FromBody]CMS_MemberCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.edit(data);
        }

    }
}

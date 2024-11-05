using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.FrontOffice;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterTimeSettingController : Controller
    {
        MasterTimeSettingDelegate ad = new MasterTimeSettingDelegate();

        // GET: api/Academic/5

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterTimeSettingDTO Get(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return ad.getdetails(id);
        }

        // POST: api/Academic
        //[HttpPost]
        //public TT_Category_Class_DTO Post([FromBody]TT_Category_Class_DTO ac)
        //{

        //    //Int32 ClassCategoryID = 0;

        //    //if (HttpContext.Session.GetString("ClassCategoryID") != null)
        //    //{
        //    //    ClassCategoryID = Convert.ToInt32(HttpContext.Session.GetString("ClassCategoryID"));
        //    //}

        //    //ac.TTCC_Id = ClassCategoryID;
        //    //HttpContext.Session.Remove("ClassCategoryID");

        //    int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
        //    ac.roleId = roleidd;
        //    ac.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //   // ac.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
        //    ac.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    return ad.savedetails(ac);
        //}
        [HttpPost]
        [Route("savedetail")]
        public MasterTimeSettingDTO getdetail([FromBody]MasterTimeSettingDTO ac)
        {
            // HttpContext.Session.SetString("ClassCategoryID", id.ToString());
            ac.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savedetail(ac);
        }

        //[Route("deletedetails/{id:int}")]
        //public TT_Category_Class_DTO Delete(int id)
        //{
        //    TT_Category_Class_DTO dto = new TT_Category_Class_DTO();
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //   // dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
        //    dto.TTCC_Id = id;
        //    int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
        //    dto.roleId = roleidd;
        //    return ad.deleterec(dto);
        //}


    }
}

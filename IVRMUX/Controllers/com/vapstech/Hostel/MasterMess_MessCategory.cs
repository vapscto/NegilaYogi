using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Hostel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Hostel
{
    [Route("api/[controller]")]
    public class MasterMess_MessCategory : Controller
    {
        MasterMess_MessCategoryDelegate TMD = new MasterMess_MessCategoryDelegate();
        [HttpGet]
        [Route("get_Mmessdata/{id:int}")]
        public MasterMess_MessCategoryDTO get_Mmessdata(int id)
        {
            MasterMess_MessCategoryDTO data = new MasterMess_MessCategoryDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.get_Mmessdata(data);
        }
        [HttpPost]
        [Route("save_Mmessdata")]
        public HL_Master_Mess_DTO save_Mmessdata([FromBody]HL_Master_Mess_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.save_Mmessdata(data);
        }

        [Route("edit_Mmessdata")]
        public MasterMess_MessCategoryDTO edit_Mmessdata([FromBody]MasterMess_MessCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.edit_Mmessdata(data);
        }

        [Route("deactiveY_Mmessdata")]
        public MasterMess_MessCategoryDTO deactiveY_Mmessdata([FromBody]MasterMess_MessCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deactiveY_Mmessdata(data);
        }
    }
}

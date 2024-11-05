using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory
{
    [Route("api/[controller]")]
    public class INVMasterCategoryController : Controller
    {
        INVMasterCategoryDelegate _delegate = new INVMasterCategoryDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INVMasterCategoryDTO getloaddata(int id)
        {
            INVMasterCategoryDTO data = new INVMasterCategoryDTO();
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INVMasterCategoryDTO savedetails([FromBody] INVMasterCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }
       
        [Route("deactive")]
        public INVMasterCategoryDTO deactive([FromBody] INVMasterCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }
         [Route("getorder")]
        public INVMasterCategoryDTO getorder(INVMasterCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getorder(data);
        }
        [Route("saveorder")]
        public INVMasterCategoryDTO saveorder([FromBody] INVMasterCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.saveorder(data);
        }

        
    }
}

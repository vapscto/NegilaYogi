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
    public class INV_PhyStock_UpdationController : Controller
    {
        INV_PhyStock_UpdationDelegate _delegate = new INV_PhyStock_UpdationDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_PhyStock_UpdationDTO getloaddata(int id)
        {
            INV_PhyStock_UpdationDTO data = new INV_PhyStock_UpdationDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
      
        [Route("savedetails")]
        public INV_PhyStock_UpdationDTO savedetails([FromBody] INV_PhyStock_UpdationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }
       
        [Route("deactive")]
        public INV_PhyStock_UpdationDTO deactive([FromBody] INV_PhyStock_UpdationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        [Route("getobdetails")]
        public INV_PhyStock_UpdationDTO getobdetails([FromBody] INV_PhyStock_UpdationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getobdetails(data);
        }

        
    }
}

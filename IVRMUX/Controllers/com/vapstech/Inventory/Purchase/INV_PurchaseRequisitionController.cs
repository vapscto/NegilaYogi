using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory
{
    [Route("api/[controller]")]
    public class INV_PurchaseRequisitionController : Controller
    {
        INV_PurchaseRequisitionDelegate _delegate = new INV_PurchaseRequisitionDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_PurchaseRequisitionDTO getloaddata(int id)
        {
            INV_PurchaseRequisitionDTO data = new INV_PurchaseRequisitionDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getloaddata(data);
        }
        [Route("get_prdetails")]
        public INV_PurchaseRequisitionDTO get_prdetails([FromBody] INV_PurchaseRequisitionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_prdetails(data);
        }

        [Route("getitemDetail")]
        public INV_PurchaseRequisitionDTO getitemDetail([FromBody] INV_PurchaseRequisitionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemDetail(data);
        }

        [Route("savedetails")]
        public INV_PurchaseRequisitionDTO savedetails([FromBody] INV_PurchaseRequisitionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleflag = Convert.ToString(HttpContext.Session.GetString("RoleNme"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            //if (data.Roleflag == "Student")
            //{
            //    data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            //}

            return _delegate.savedetails(data);
        }
        [Route("edit")]
        public INV_PurchaseRequisitionDTO edit([FromBody] INV_PurchaseRequisitionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delegate.edit(data);
        }

        [Route("deactive")]
        public INV_PurchaseRequisitionDTO deactive([FromBody] INV_PurchaseRequisitionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }

        [Route("deactiveM")]
        public INV_PurchaseRequisitionDTO deactiveM([FromBody] INV_PurchaseRequisitionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveM(data);
        }






    }
}

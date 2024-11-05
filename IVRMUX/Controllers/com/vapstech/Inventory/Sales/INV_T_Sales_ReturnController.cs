using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory;
using IVRMUX.Delegates.com.vapstech.Inventory.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Sales
{

    [Route("api/[controller]")]
    public class INV_T_Sales_ReturnController : Controller
    {
        public INV_T_Sales_ReturnDelegate _delegate = new INV_T_Sales_ReturnDelegate();
        [Route("getloaddata/{id:int}")]
        public INV_T_SalesReturnDTO getloaddata(int id)
        {
            INV_T_SalesReturnDTO data = new INV_T_SalesReturnDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getitem")]
        public INV_T_SalesReturnDTO getitem([FromBody] INV_T_SalesReturnDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitem(data);
        }
        [Route("getitemDetail")]
        public INV_T_SalesReturnDTO getitemDetail([FromBody] INV_T_SalesReturnDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemDetail(data);
        }

        [Route("savedetails")]
        public INV_T_SalesReturnDTO savedetails([FromBody] INV_T_SalesReturnDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }
        [Route("deactive")]
        public INV_T_SalesReturnDTO deactive([FromBody] INV_T_SalesReturnDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        [Route("viewitem")]
        public INV_T_SalesReturnDTO viewitem([FromBody] INV_T_SalesReturnDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.viewitem(data);
        }




    }
}
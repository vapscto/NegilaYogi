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
    public class INV_T_SalesController : Controller
    {
        INV_T_SalesDelegate _delegate = new INV_T_SalesDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_T_SalesDTO getloaddata(int id)
        {
            INV_T_SalesDTO data = new INV_T_SalesDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("getStudentClsSec")]
        public INV_T_SalesDTO getStudentClsSec([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getStudentClsSec(data);
        }
        [Route("getsectionlist")]
        public INV_T_SalesDTO getsectionlist([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getsectionlist(data);
        }
        [Route("getStudentlist")]
        public INV_T_SalesDTO getStudentlist([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getStudentlist(data);
        }
        [Route("getitem")]
        public INV_T_SalesDTO getitem([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitem(data);
        }
        [Route("getitemDetail")]
        public INV_T_SalesDTO getitemDetail([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return _delegate.getitemDetail(data);
        }

        [Route("savedetails")]
        public INV_T_SalesDTO savedetails([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.savedetails(data);
        }
        [Route("getSaletypes")]
        public INV_T_SalesDTO getSaletypes([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));       
            return _delegate.getSaletypes(data);
        }
        [Route("getSaleItemDetails")]
        public INV_T_SalesDTO getSaleItemDetails([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getSaleItemDetails(data);
        }
        [Route("getSaleItemTax")]
        public INV_T_SalesDTO getSaleItemTax([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getSaleItemTax(data);
        }
        [Route("deactive")]
        public INV_T_SalesDTO deactive([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IMFY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("IMFY_Id"));
            return _delegate.deactive(data);
        }
        [Route("deactiveS")]
        public INV_T_SalesDTO deactiveS([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveS(data);
        }
        [Route("deactivetax")]
        public INV_T_SalesDTO deactivetax([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactivetax(data);
        }



    }
}

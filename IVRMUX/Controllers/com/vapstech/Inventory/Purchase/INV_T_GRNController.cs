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
    public class INV_T_GRNController : Controller
    {
        INV_T_GRNDelegate _delegate = new INV_T_GRNDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_T_GRNDTO getloaddata(int id)
        {
            INV_T_GRNDTO data = new INV_T_GRNDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("getitemDetail")]
        public INV_T_GRNDTO getitemDetail([FromBody] INV_T_GRNDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemDetail(data);
        }
        [Route("savedetails")]
        public INV_T_GRNDTO savedetails([FromBody] INV_T_GRNDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.savedetails(data);
        }
        [Route("get_GRNitemDetails")]
        public INV_T_GRNDTO get_GRNitemDetails([FromBody] INV_T_GRNDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_GRNitemDetails(data);
        }
        [Route("deactiveg")]
        public INV_T_GRNDTO deactiveg([FromBody] INV_T_GRNDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveg(data);
        }
        [Route("get_itemtax")]
        public INV_T_GRNDTO get_itemtax([FromBody] INV_T_GRNDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_itemtax(data);
        }
        [Route("deactivet")]
        public INV_T_GRNDTO deactivet([FromBody] INV_T_GRNDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactivet(data);
        }

        [Route("deactive")]
        public INV_T_GRNDTO deactive([FromBody] INV_T_GRNDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        [Route("Edit_GRN_details")]
        public INV_T_GRNDTO Edit_GRN_details([FromBody] INV_T_GRNDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.Edit_GRN_details(data);
        }
        //[Route("SearchByColumn")]
        //public INV_T_GRNDTO SearchByColumn([FromBody] INV_T_GRNDTO data)
        //{
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return _delegate.SearchByColumn(data);
        //}








    }
}

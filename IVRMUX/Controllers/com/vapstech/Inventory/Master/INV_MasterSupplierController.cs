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
    public class INV_MasterSupplierController : Controller
    {
        INV_MasterSupplierDelegate _delegate = new INV_MasterSupplierDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_Master_SupplierDTO getloaddata(int id)
        {
            INV_Master_SupplierDTO data = new INV_Master_SupplierDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_Master_SupplierDTO savedetails([FromBody] INV_Master_SupplierDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }
       
        [Route("deactive")]
        public INV_Master_SupplierDTO deactive([FromBody] INV_Master_SupplierDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }

        
    }
}

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
    public class INV_MasterTaxController : Controller
    {
        INV_MasterTaxDelegate _delegate = new INV_MasterTaxDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_Master_TaxDTO getloaddata(int id)
        {
            INV_Master_TaxDTO data = new INV_Master_TaxDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_Master_TaxDTO savedetails([FromBody] INV_Master_TaxDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }
       
        [Route("deactive")]
        public INV_Master_TaxDTO deactive([FromBody] INV_Master_TaxDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }

        
    }
}

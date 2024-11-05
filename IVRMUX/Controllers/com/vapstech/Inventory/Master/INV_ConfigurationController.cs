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
    public class INV_ConfigurationController : Controller
    {
        INV_ConfigurationDelegate _delegate = new INV_ConfigurationDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public INV_ConfigurationDTO getloaddata(int id)
        {
            INV_ConfigurationDTO data = new INV_ConfigurationDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_ConfigurationDTO savedetails([FromBody] INV_ConfigurationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }
           
    }
}

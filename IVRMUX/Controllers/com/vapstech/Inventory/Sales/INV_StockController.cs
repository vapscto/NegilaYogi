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
    public class INV_StockController : Controller
    {
        INV_StockDelegate _delegate = new INV_StockDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_StockDTO getloaddata(int id)
        {
            INV_StockDTO data = new INV_StockDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
      
        [Route("savedetails")]
        public INV_StockDTO savedetails([FromBody] INV_StockDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }
        [Route("editStock")]
        public INV_StockDTO editStock([FromBody] INV_StockDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.editStock(data);
        }

        

    }
}

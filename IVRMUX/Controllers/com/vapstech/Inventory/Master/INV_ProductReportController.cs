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
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class INV_ProductReportController : Controller
    {

        INV_ProductReportDelegate inv = new INV_ProductReportDelegate();
     

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public INV_Master_ProductDTO getalldetails(int id)
        {
            INV_Master_ProductDTO data = new INV_Master_ProductDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return inv.getalldetails(data);

        }

        [Route("radiobtndata")]
        public INV_Master_ProductDTO radiobtndata([FromBody]INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return inv.radiobtndata(data);

        }

        

        [Route("getdata")]
        public INV_Master_ProductDTO getdata([FromBody]INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return inv.getdata(data);

        }


    }
}

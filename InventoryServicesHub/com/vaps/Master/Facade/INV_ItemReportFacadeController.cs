using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using InventoryServicesHub.com.vaps.Master.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Master.Controllers
{
    [Route("api/[controller]")]
    public class INV_ItemReportFacadeController : Controller
    {
        
        INV_ItemReportInterface _Inv;
        public INV_ItemReportFacadeController(INV_ItemReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_Master_ItemDTO> getloaddata([FromBody] INV_Master_ItemDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<INV_Master_ItemDTO> onreport([FromBody] INV_Master_ItemDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}

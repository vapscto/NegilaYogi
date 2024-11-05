using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class INV_DashboardFacadeController : Controller
    {
        // GET: api/values
        INV_DashboardInterface _Inv;
        public INV_DashboardFacadeController(INV_DashboardInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_DashboardDTO> getloaddata([FromBody] INV_DashboardDTO data)
        {
            return _Inv.getloaddata(data);
        }
       [Route("getwarrantydetails")]
        public Task<INV_DashboardDTO> getwarrantydetails([FromBody] INV_DashboardDTO data)
        {
            return _Inv.getwarrantydetails(data);
        }
       
        


    }
}

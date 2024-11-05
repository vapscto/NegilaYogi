using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Sales.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Sales.Controllers
{
    [Route("api/[controller]")]
    public class INV_MonthEndReportFacadeController : Controller
    {
        // GET: api/values
        INV_MonthEndReportInterface _Inv;
        public INV_MonthEndReportFacadeController(INV_MonthEndReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_MonthEndReportDTO getloaddata([FromBody] INV_MonthEndReportDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("getmonthreport")]
        public Task<INV_MonthEndReportDTO> getmonthreport([FromBody] INV_MonthEndReportDTO data)
        {
            return _Inv.getmonthreport(data);
        }
      

    }
}

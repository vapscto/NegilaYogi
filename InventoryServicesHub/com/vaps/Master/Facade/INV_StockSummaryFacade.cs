using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Master.Facade
{
    [Route("api/[controller]")]
    public class INV_StockSummaryFacade : Controller
    {
        INV_StockSummaryInterface _Inv;
        public INV_StockSummaryFacade(INV_StockSummaryInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_StockSummaryDTO> getloaddata([FromBody] INV_StockSummaryDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<INV_StockSummaryDTO> onreport([FromBody] INV_StockSummaryDTO data)
        {
            return _Inv.onreport(data);
        }
        //onreporttwo
        [Route("onreporttwo")]
        public Task<INV_StockSummaryDTO> onreporttwo([FromBody] INV_StockSummaryDTO data)
        {
            return _Inv.onreporttwo(data);
        }
        //onreportthree
        [Route("onreportthree")]
        public Task<INV_StockSummaryDTO> onreportthree([FromBody] INV_StockSummaryDTO data)
        {
            return _Inv.onreportthree(data);
        }
        //get_load_onchange
        [Route("get_load_onchange")]
        public Task<INV_StockSummaryDTO> get_load_onchange([FromBody] INV_StockSummaryDTO data)
        {
            return _Inv.load_onchange(data);
        }
        //getstudent
        [Route("getstudent")]
        public Task<INV_StockSummaryDTO> getstudent([FromBody] INV_StockSummaryDTO data)
        {
            return _Inv.getstudent(data);
        }
        //onreportstock
        [Route("onreportstock")]
        public Task<INV_StockSummaryDTO> onreportstock([FromBody] INV_StockSummaryDTO data)
        {
            return _Inv.onreportstock(data);
        }
    }
}

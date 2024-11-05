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
    public class INV_R_SalesFacadeController : Controller
    {
        // GET: api/values
        INV_R_SalesInterface _Inv;
        public INV_R_SalesFacadeController(INV_R_SalesInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_T_SalesDTO getloaddata([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("mainradiochange")]
        public Task<INV_T_SalesDTO> mainradiochange([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.mainradiochange(data);
        }
        [Route("radiochange")]
        public INV_T_SalesDTO radiochange([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.radiochange(data);
        }
        [Route("getStudentlist")]
        public Task<INV_T_SalesDTO> getStudentlist([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.getStudentlist(data);
        }



        [Route("onreport")]
        public Task<INV_T_SalesDTO> onreport([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.onreport(data);
        }




    }
}

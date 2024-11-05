using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Purchase.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Purchase.Controllers
{
    [Route("api/[controller]")]
    public class INV_R_GRNFacadeController : Controller
    {
        // GET: api/values
        INV_R_GRNInterface _Inv;
        public INV_R_GRNFacadeController(INV_R_GRNInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_T_GRNDTO> getloaddata([FromBody] INV_T_GRNDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<INV_T_GRNDTO> onreport([FromBody] INV_T_GRNDTO data)
        {
            return _Inv.onreport(data);
        }

        [Route("getdata_ob")]
        public INV_OpeningBalanceDTO getdata_ob([FromBody] INV_OpeningBalanceDTO data)
        {
            return _Inv.getdata_ob(data);
        }
        [Route("GetReport_ob")]
        public INV_OpeningBalanceDTO GetReport_ob([FromBody] INV_OpeningBalanceDTO data)
        {
            return _Inv.GetReport_ob(data);
        }


    }
}

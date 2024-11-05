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
    public class INV_MasterTaxFacadeController : Controller
    {
        // GET: api/values
        INV_MasterTaxInterface _Inv;
        public INV_MasterTaxFacadeController(INV_MasterTaxInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_Master_TaxDTO getloaddata([FromBody] INV_Master_TaxDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [HttpPost]
        [Route("savedetails")]
        public INV_Master_TaxDTO savedetails([FromBody] INV_Master_TaxDTO data)
        {
            return _Inv.savedetails(data);
        }
       
        [Route("deactive")]
        public INV_Master_TaxDTO deactive([FromBody] INV_Master_TaxDTO data)
        {
            return _Inv.deactive(data);
        }
        


    }
}

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
    public class INVMasterCategoryFacadeController : Controller
    {
        // GET: api/values
        INVMasterCategoryInterface _Inv;
        public INVMasterCategoryFacadeController(INVMasterCategoryInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INVMasterCategoryDTO getloaddata([FromBody] INVMasterCategoryDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [HttpPost]
        [Route("savedetails")]
        public INVMasterCategoryDTO savedetails([FromBody] INVMasterCategoryDTO data)
        {
            return _Inv.savedetails(data);
        }
       
        [Route("deactive")]
        public INVMasterCategoryDTO deactive([FromBody] INVMasterCategoryDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("getorder")]
        public INVMasterCategoryDTO getorder([FromBody] INVMasterCategoryDTO data)
        {
            return _Inv.getorder(data);
        }
        [Route("saveorder")]
        public INVMasterCategoryDTO saveorder([FromBody] INVMasterCategoryDTO data)
        {
            return _Inv.saveorder(data);
        }
        


    }
}

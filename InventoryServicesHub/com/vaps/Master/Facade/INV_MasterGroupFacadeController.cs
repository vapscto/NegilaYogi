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
    public class INV_MasterGroupFacadeController : Controller
    {
        // GET: api/values
        INV_MasterGroupInterface _Inv;
        public INV_MasterGroupFacadeController(INV_MasterGroupInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_Master_GroupDTO getloaddata([FromBody] INV_Master_GroupDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [HttpPost]
        [Route("savedetails")]
        public INV_Master_GroupDTO savedetails([FromBody] INV_Master_GroupDTO data)
        {
            return _Inv.savedetails(data);
        }
        [HttpPost]
        [Route("savedetailsUG")]
        public INV_Master_GroupDTO savedetailsUG([FromBody] INV_Master_GroupDTO data)
        {
            return _Inv.savedetailsUG(data);
        }
        [HttpPost]
        [Route("savedetailsIG")]
        public INV_Master_GroupDTO savedetailsIG([FromBody] INV_Master_GroupDTO data)
        {
            return _Inv.savedetailsIG(data);
        }
        [Route("deactive")]
        public INV_Master_GroupDTO deactive([FromBody] INV_Master_GroupDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("groupChange")]
        public INV_Master_GroupDTO groupChange([FromBody] INV_Master_GroupDTO data)
        {
            return _Inv.groupChange(data);
        }
        [Route("usergroup")]
        public INV_Master_GroupDTO usergroup([FromBody] INV_Master_GroupDTO data)
        {
            return _Inv.usergroup(data);
        }
        [Route("Itemgroup")]
        public INV_Master_GroupDTO Itemgroup([FromBody] INV_Master_GroupDTO data)
        {
            return _Inv.Itemgroup(data);
        }
    

        

    }
}

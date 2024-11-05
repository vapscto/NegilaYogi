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
    [Route("api/[controller]")]
    public class INV_MasterGroupController : Controller
    {
        INV_MasterGroupDelegate _delegate = new INV_MasterGroupDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_Master_GroupDTO getloaddata(int id)
        {
            INV_Master_GroupDTO data = new INV_Master_GroupDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_Master_GroupDTO savedetails([FromBody] INV_Master_GroupDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }
        [Route("savedetailsUG")]
        public INV_Master_GroupDTO savedetailsUG([FromBody] INV_Master_GroupDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetailsUG(data);
        }
        [Route("savedetailsIG")]
        public INV_Master_GroupDTO savedetailsIG([FromBody] INV_Master_GroupDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetailsIG(data);
        }
        [Route("deactive")]
        public INV_Master_GroupDTO deactive([FromBody] INV_Master_GroupDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        [Route("groupChange")]
        public INV_Master_GroupDTO groupChange([FromBody] INV_Master_GroupDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.groupChange(data);
        }
        [Route("usergroup")]
        public INV_Master_GroupDTO usergroup([FromBody] INV_Master_GroupDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.usergroup(data);
        }
        [Route("Itemgroup")]
        public INV_Master_GroupDTO Itemgroup([FromBody] INV_Master_GroupDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.Itemgroup(data);
        }
        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.TT
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterBuildingController : Controller
    {
        MasterBuildingDelegate objdelegate = new MasterBuildingDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public TT_Master_BuildingDTO Get([FromQuery] int id)
        {

            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(id);
        }


        [HttpPost]
        [Route("savedetail")]
        public TT_Master_BuildingDTO savedetail([FromBody] TT_Master_BuildingDTO periodpage)
        {
            periodpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(periodpage);
        }

        [HttpPost]
        [Route("savedetail1")]
        public TT_Master_BuildingDTO savedetail1([FromBody] TT_Master_BuildingDTO periodpage)
        {
            periodpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail1(periodpage);
        }

        [HttpGet]
        [Route("getpagedetails/{id:long}")]
        public TT_Master_BuildingDTO getpagedetails(long id)
        {
            return objdelegate.getpagedetails(id);
        }

        [HttpGet]
        [Route("getpagedetails1/{id:long}")]
        public TT_Master_BuildingDTO getpagedetails1(long id)
        {
            return objdelegate.getpagedetails1(id);
        }

        [HttpPost]
        [Route("deactivate")]
        public TT_Master_BuildingDTO deactivate([FromBody] TT_Master_BuildingDTO id)
        {
            return objdelegate.deactivate(id);
        }

        [HttpPost]
        [Route("deactivate1")]
        public TT_Master_BuildingDTO deactivate1([FromBody] TT_Master_BuildingDTO id)
        {
            return objdelegate.deactivate1(id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTableServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
        public class MasterBuildingFacadeController : Controller
        {
            public MasterBuildingInterface _ttmasterbuilding;

            public MasterBuildingFacadeController(MasterBuildingInterface maspag)
            {
                 _ttmasterbuilding = maspag;
            }


            // GET: api/values
            [HttpGet]
            public IEnumerable<string> Get()
            {
                return new string[] { "value1", "value2" };
            }

            // GET api/values/5
            [Route("getdetails/{id:int}")]
            public TT_Master_BuildingDTO getdetails(int id)
            {
                return _ttmasterbuilding.getdetails(id);
            }
            
            [HttpPost]
            [Route("savedetail")]
            public TT_Master_BuildingDTO Post([FromBody] TT_Master_BuildingDTO org)
            {
                return _ttmasterbuilding.savedetail(org);
            }

            [HttpPost]
            [Route("savedetail1")]
            public TT_Master_BuildingDTO Post1([FromBody] TT_Master_BuildingDTO org)
            {
                return _ttmasterbuilding.savedetail1(org);
            }
            [HttpGet]
            [Route("getpagedetails/{id:long}")]
            public TT_Master_BuildingDTO getpagedetails(long id)
            {
                return _ttmasterbuilding.getpagedetails(id);
            }
            [HttpGet]
            [Route("getpagedetails1/{id:long}")]
            public TT_Master_BuildingDTO getpagedetails1(long id)
            {
                return _ttmasterbuilding.getpagedetails1(id);
            }
            [HttpPost]
            [Route("deactivate")]
            public TT_Master_BuildingDTO deactivate([FromBody] TT_Master_BuildingDTO id)
            {
                // id = 12;
                return _ttmasterbuilding.deactivate(id);
            }
            [HttpPost]
            [Route("deactivate1")]
            public TT_Master_BuildingDTO deactivate1([FromBody] TT_Master_BuildingDTO id)
            {
                // id = 12;
                return _ttmasterbuilding.deactivate1(id);
            }
    }
}

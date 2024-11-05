using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlacementServiceHub.com.Interfaces;
using PreadmissionDTOs.com.vaps.Placement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlacementServiceHub.com.Controllers
{
    [Route("api/[controller]")]
    public class PlacementJobScheduleTitleFacade : Controller
    {
        public PlacementJobScheduleTitleInterface _interface;

        public PlacementJobScheduleTitleFacade(PlacementJobScheduleTitleInterface _inter)
        {
            _interface = _inter;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata")]
        public PlacementJobScheduleTitleDTO loaddata([FromBody] PlacementJobScheduleTitleDTO data)
        {
            return _interface.loaddata(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public PlacementJobScheduleTitleDTO savedetails([FromBody] PlacementJobScheduleTitleDTO data)
        {
            return _interface.savedetails(data);
        }
        [Route("editdetails")]
        public PlacementJobScheduleTitleDTO editdetails([FromBody] PlacementJobScheduleTitleDTO data)
        {
            return _interface.editdetails(data);
        }
        [Route("deactive")]
        public PlacementJobScheduleTitleDTO deactive([FromBody] PlacementJobScheduleTitleDTO data)
        {
            return _interface.deactive(data);
        }

        [Route("report")]
        public PlacementJobScheduleTitleDTO report([FromBody] PlacementJobScheduleTitleDTO data)
        {
            return _interface.report(data);
        }

    }
}

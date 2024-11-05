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
    public class PL_CI_StudentStatusFacade : Controller
    {
        public PL_CI_StudentStatusInterface _interface;

        public PL_CI_StudentStatusFacade(PL_CI_StudentStatusInterface _inter)
        {
            _interface = _inter;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata")]
        public PL_CI_StudentStatusDTO loaddata([FromBody] PL_CI_StudentStatusDTO data)
        {
            return _interface.loaddata(data);
        }


        [HttpPost]
        [Route("savedetails")]
        public PL_CI_StudentStatusDTO savedetails([FromBody] PL_CI_StudentStatusDTO data)
        {
            return _interface.savedetails(data);
        }

        [Route("editdetails")]
        public PL_CI_StudentStatusDTO editdetails([FromBody] PL_CI_StudentStatusDTO data)
        {
            return _interface.editdetails(data);
        }
        [Route("deactive")]
        public PL_CI_StudentStatusDTO deactive([FromBody] PL_CI_StudentStatusDTO data)
        {
            return _interface.deactive(data);
        }

    }
}

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
    public class PL_CI_Schedule_CompanyFacade : Controller
    {
        public PL_CI_Schedule_CompanyInterface _cms;

        public PL_CI_Schedule_CompanyFacade(PL_CI_Schedule_CompanyInterface cmsdept)
        {
            _cms = cmsdept;
        }
        //[HttpGet]
        //[Route("loaddata/{id:int}")]
        //public PL_CI_Schedule_CompanyDTO loaddata(int id)
        //{
        //    return _cms.loaddata(id);
        //    // return _cms.loaddata(id);
        //}
        //[HttpPost]
        //[Route("savedata")]
        //public PL_CI_Schedule_CompanyDTO savedata([FromBody]PL_CI_Schedule_CompanyDTO data)
        //{
        //    return _cms.savedata(data);
        //}
        [HttpPost]
        [Route("loaddata")]
        public PL_CI_Schedule_CompanyDTO loaddata([FromBody] PL_CI_Schedule_CompanyDTO data)
        {
            return _cms.loaddata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public PL_CI_Schedule_CompanyDTO savedata([FromBody] PL_CI_Schedule_CompanyDTO data)
        {
            return _cms.savedata(data);
        }
        [Route("deactive")]
        public PL_CI_Schedule_CompanyDTO deactive([FromBody] PL_CI_Schedule_CompanyDTO data)
        {
            return _cms.deactive(data);
        }
        [Route("editdetails")]        public PL_CI_Schedule_CompanyDTO editdetails([FromBody] PL_CI_Schedule_CompanyDTO data)        {            return _cms.editdetails(data);        }


    }
}

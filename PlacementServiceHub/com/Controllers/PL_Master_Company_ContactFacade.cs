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
    public class PL_Master_Company_ContactFacade : Controller
    {
        public PL_Master_Company_ContactInterface _cms;

        public PL_Master_Company_ContactFacade(PL_Master_Company_ContactInterface cmsdept)
        {
            _cms = cmsdept;
        }
        //[HttpGet]
        //[Route("loaddata/{id:int}")]
        //public PL_Master_Company_ContactDTO loaddata(int id)
        //{
        //    return _cms.loaddata(id);
        //    // return _cms.loaddata(id);
        //}
        //[HttpPost]
        //[Route("savedata")]
        //public PL_Master_Company_ContactDTO savedata([FromBody]PL_Master_Company_ContactDTO data)
        //{
        //    return _cms.savedata(data);
        //}
        [HttpPost]
        [Route("loaddata")]
        public PL_Master_Company_ContactDTO loaddata([FromBody] PL_Master_Company_ContactDTO data)
        {
            return _cms.loaddata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public PL_Master_Company_ContactDTO savedata([FromBody] PL_Master_Company_ContactDTO data)
        {
            return _cms.savedata(data);
        }
        [Route("deactive")]
        public PL_Master_Company_ContactDTO deactive([FromBody] PL_Master_Company_ContactDTO data)
        {
            return _cms.deactive(data);
        }


    }
}

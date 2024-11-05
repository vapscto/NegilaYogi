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
    public class Master_CISFacade : Controller
    {
        public Master_CISInterface _cms;

        public Master_CISFacade(Master_CISInterface cmsdept)
        {
            _cms = cmsdept;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public PL_CampusInterview_ScheduleDTO loaddata(int id)
        {
            return _cms.loaddata(id);
            // return _cms.loaddata(id);
        }
        [HttpPost]
        [Route("savedata")]
        public PL_CampusInterview_ScheduleDTO savedata([FromBody]PL_CampusInterview_ScheduleDTO data)
        {
            return _cms.savedata(data);
        }

        //edit
        [Route("edit")]
        public PL_CampusInterview_ScheduleDTO edit([FromBody]PL_CampusInterview_ScheduleDTO data)
        {
            return _cms.edit(data);
        }
        //deactive
        [Route("deactive")]
        public PL_CampusInterview_ScheduleDTO deactive([FromBody]PL_CampusInterview_ScheduleDTO data)
        {
            return _cms.deactive(data);
        }
    }
}

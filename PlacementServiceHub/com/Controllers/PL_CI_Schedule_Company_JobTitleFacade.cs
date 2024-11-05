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
    public class PL_CI_Schedule_Company_JobTitleFacade : Controller
    {
        public PL_CI_Schedule_Company_JobTitleInterface _cms;

        public PL_CI_Schedule_Company_JobTitleFacade(PL_CI_Schedule_Company_JobTitleInterface cmsdept)
        {
            _cms = cmsdept;
        }

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public PL_CI_Schedule_Company_JobTitleDTO loaddata(int id)
        {
            return _cms.loaddata(id);
            
        }

        [Route("saveRecord")]
        public PL_CI_Schedule_Company_JobTitleDTO saveRecord([FromBody]PL_CI_Schedule_Company_JobTitleDTO data)
        {
            return _cms.saveRecord(data);
        }

        [Route("EditDetails")]
        public PL_CI_Schedule_Company_JobTitleDTO EditDetails([FromBody]PL_CI_Schedule_Company_JobTitleDTO data)
        {
            return _cms.EditDetails(data);
        }

        [Route("deactivate")]
        public PL_CI_Schedule_Company_JobTitleDTO deactivate([FromBody]PL_CI_Schedule_Company_JobTitleDTO data)
        {
            return _cms.deactivate(data);
        }
    }
}

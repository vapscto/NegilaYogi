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
    public class PL_CI_Schedule_Company_JobTitle_CourseBranchFacade : Controller
    {
        public PL_CI_Schedule_Company_JobTitle_CourseBranchInterface _cms;

        public PL_CI_Schedule_Company_JobTitle_CourseBranchFacade(PL_CI_Schedule_Company_JobTitle_CourseBranchInterface cmsdept)
        {
            _cms = cmsdept;
        }

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO loaddata(int id)
        {
            return _cms.loaddata(id);

        }

        [Route("saveRecord")]
        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO saveRecord([FromBody]PL_CI_Schedule_Company_JobTitle_CourseBranchDTO data)
        {
            return _cms.saveRecord(data);
        }

        [Route("EditDetails")]
        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO EditDetails([FromBody]PL_CI_Schedule_Company_JobTitle_CourseBranchDTO data)
        {
            return _cms.EditDetails(data);
        }

        [Route("deactivate")]
        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO deactivate([FromBody]PL_CI_Schedule_Company_JobTitle_CourseBranchDTO data)
        {
            return _cms.deactivate(data);
        }
    }
}

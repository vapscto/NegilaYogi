 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Placement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Placement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Placement
{
    [Route("api/[controller]")]
    public class PL_CI_Schedule_Company_JobTitle_CourseBranch : Controller
    {
        PL_CI_Schedule_Company_JobTitle_CourseBranchDelegate cms = new PL_CI_Schedule_Company_JobTitle_CourseBranchDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.loaddata(id);
        }

        [Route("saveRecord")]
        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO saveRecord([FromBody]PL_CI_Schedule_Company_JobTitle_CourseBranchDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.saveRecord(data);
        }

        [Route("EditDetails")]
        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO EditDetails([FromBody]PL_CI_Schedule_Company_JobTitle_CourseBranchDTO data)
        {
            return cms.EditDetails(data);
        }


        [Route("deactivate")]
        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO deactivate([FromBody] PL_CI_Schedule_Company_JobTitle_CourseBranchDTO d)
        {
            //d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.deactivate(d);
        }

    }
}

﻿  using System;
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
    public class PL_CI_Schedule_Company_JobTitle_Criteria : Controller
    {
        PL_CI_Schedule_Company_JobTitle_CriteriaDelegate cms = new PL_CI_Schedule_Company_JobTitle_CriteriaDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.loaddata(id);
        }


        [Route("saveRecord")]
        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO saveRecord([FromBody]PL_CI_Schedule_Company_JobTitle_CriteriaDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.save(data);
        }

        [Route("EditDetails")]
        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO EditDetails([FromBody]PL_CI_Schedule_Company_JobTitle_CriteriaDTO data)
        {
            return cms.EditDetails(data);
        }

        [Route("deactivate")]
        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO deactivate([FromBody] PL_CI_Schedule_Company_JobTitle_CriteriaDTO d)
        {
            //d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.deactivate(d);
        }
    }
}

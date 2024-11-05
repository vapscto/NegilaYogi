﻿using System;
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
    public class Master_CIS : Controller
    {
        Master_CISDelgate cms = new Master_CISDelgate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public PL_CampusInterview_ScheduleDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return cms.loaddata(id);

        }
        [HttpPost]
        [Route("savedata")]
        public PL_CampusInterview_ScheduleDTO savedata([FromBody]PL_CampusInterview_ScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }
        //edit
        
        [Route("edit")]
        public PL_CampusInterview_ScheduleDTO edit([FromBody]PL_CampusInterview_ScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.edit(data);
        }
        //deactive
        [Route("deactive")]
        public PL_CampusInterview_ScheduleDTO deactive([FromBody]PL_CampusInterview_ScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.deactive(data);
        }
    }
}
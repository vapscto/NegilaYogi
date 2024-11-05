﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class MasterDepartmentController : Controller
    {
        MasterDepartmentDelegates _delObj = new MasterDepartmentDelegates();
        [Route("Savedata")]
        public MasterDepartmentDTO Savedata([FromBody]MasterDepartmentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.Savedata(data);
        }

      //[HttpGet]
        [Route("getdetails/{id:int}")]
        public MasterDepartmentDTO getdetails(int id)
        {
            MasterDepartmentDTO data = new MasterDepartmentDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = Convert.ToInt32(data.MI_Id);
            return _delObj.getdetails(id);
        }
        [Route("deactiveY")]
        public MasterDepartmentDTO deactiveY([FromBody]MasterDepartmentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.deactiveY(data);
        }
    }
}
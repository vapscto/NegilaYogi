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
    public class MasterFloorController : Controller
    {
        MasterFloorDelegate _delobj = new MasterFloorDelegate();
        [Route("Savedata")]
        public MasterFloorDTO Savedata([FromBody]MasterFloorDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public MasterFloorDTO getdetails(int id)
        {
            MasterFloorDTO data = new MasterFloorDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = Convert.ToInt32(data.MI_Id);
            return _delobj.getdetails(id);
        }
        [Route("deactiveY")]
        public MasterFloorDTO deactiveY([FromBody]MasterFloorDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.HOD;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.HOD
{
    [Route("api/[controller]")]
    public class HODStudentStrengthController : Controller
    {

        HODStudentStrengthDelegate crStr = new HODStudentStrengthDelegate();
        // GET: api/values
        [Route("Getdetails")]
        public ADMClassSectionStrengthDTO Getdetails(ADMClassSectionStrengthDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getdetails(data);
        }


        [Route("getclass/{id}")]
        public ADMClassSectionStrengthDTO getclass(int id)
        {
            ADMClassSectionStrengthDTO data = new ADMClassSectionStrengthDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getdetails(data);
        }
       
        [Route("Getsection")]
        public ADMClassSectionStrengthDTO Getsection([FromBody] ADMClassSectionStrengthDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getsection(data);

        }

      
        [Route("Getsectioncount")]
        public ADMClassSectionStrengthDTO Getsectioncount([FromBody] ADMClassSectionStrengthDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getsectioncount(data);

        }
    }
}

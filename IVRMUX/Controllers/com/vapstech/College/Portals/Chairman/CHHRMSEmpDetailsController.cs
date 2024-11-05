﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CHHRMSEmpDetailsController : Controller
    {
        CHHRMSEmpDetailsDelegate clgdelegate = new CHHRMSEmpDetailsDelegate();

        [HttpGet]
        [Route("Getdetails")]      
        public CHHRMSEmpSalaryDTO Getdetails(CHHRMSEmpSalaryDTO data)
        {     
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));          
            data.HRMLY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.Getdetails(data);
        }

        [Route("yrchange/{id}")]
        public CHHRMSEmpSalaryDTO yrchange(int id)
        {
            CHHRMSEmpSalaryDTO data = new CHHRMSEmpSalaryDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            data.HRMLY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.Getdetails(data);
        }

    }
}

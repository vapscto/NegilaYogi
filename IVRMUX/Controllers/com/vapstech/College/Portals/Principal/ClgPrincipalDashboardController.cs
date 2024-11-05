﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
//using DomainModel.Model.com.vapstech.p;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;
using PreadmissionDTOs.com.vaps.College.Portals;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgPrincipalDashboardController : Controller
    {


        ClgPrincipalDashboardDelegate prStr = new ClgPrincipalDashboardDelegate();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public clgChairmanDashboardDTO Getdetails(clgChairmanDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.PaymentNootificationCollegePrinicipal = Convert.ToInt64(HttpContext.Session.GetInt32("PaymentNootificationCollegePrinicipal"));
            return prStr.Getdetails(data);            
        }
        [Route("GetDataByYear/{id:int}")]
        public clgChairmanDashboardDTO GetDataByYear(int id)
        {
            clgChairmanDashboardDTO data = new clgChairmanDashboardDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            return prStr.Getdetails(data);
        }

    }

}

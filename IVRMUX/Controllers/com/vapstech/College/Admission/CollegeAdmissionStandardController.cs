using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegeAdmissionStandardController : Controller
    {
        CollegeAdmissionStandardDelegate adsd = new CollegeAdmissionStandardDelegate();


        // POST api/values
        [HttpPost]
        [Route("savedata")]
        public CollegeAdmissionStandardDTO getclassstudentlist([FromBody]CollegeAdmissionStandardDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getlisttwo(student);
        }

        [HttpGet]
        [Route("loaddata/{id:int}")]
        // [Route("loaddata")]
        public CollegeAdmissionStandardDTO getdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getlistdata(id);
        }
    }
}

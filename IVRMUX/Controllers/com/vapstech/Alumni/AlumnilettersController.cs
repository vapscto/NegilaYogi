using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Alumni;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.Alumni;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Alumni
{
    [Route("api/[controller]")]
    public class AlumnilettersController : Controller
    {
        AlumnilettersDelegate delg = new AlumnilettersDelegate();


         [Route("ShowReport")]
        public AlumnilettersDTO ShowReport([FromBody] AlumnilettersDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delg.ShowReport(MMD);
        }
        
         [Route("BindData/{id:int}")]
        public AlumnilettersDTO BindData(int id)
        {
            AlumnilettersDTO data = new AlumnilettersDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delg.BindData(data);
        }
     
         [Route("letterReport")]
        public AlumnilettersDTO letterReport([FromBody] AlumnilettersDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delg.letterReport(data);
        }
    }
}

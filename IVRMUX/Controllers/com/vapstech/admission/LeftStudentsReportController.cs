using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class LeftStudentsReportController : Controller
    {
        LeftStudentsReportDelegate _delg = new LeftStudentsReportDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        [Route("loaddata/{id:int}")]
        public LeftStudentsReportDTO loaddata(int id)
        {
            LeftStudentsReportDTO data = new LeftStudentsReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.loaddata(data);
        }
        //getCategory
        [Route("getCategory")]
        public LeftStudentsReportDTO getCategory([FromBody] LeftStudentsReportDTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getCategory(data);
        }
        //getClass
        [Route("getClass")]
        public LeftStudentsReportDTO getClass([FromBody] LeftStudentsReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getClass(data);
        }
        //getsection
        [Route("getsection")]
        public LeftStudentsReportDTO getsection([FromBody] LeftStudentsReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getsection(data);
        }
        //report
        [Route("report")]
        public LeftStudentsReportDTO report([FromBody] LeftStudentsReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.report(data);
        }
    }
}

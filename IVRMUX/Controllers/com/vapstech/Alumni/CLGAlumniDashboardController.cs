using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Alumni;
using IVRMUX.Delegates.com.vapstech.Alumni;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Alumni
{
    [Route("api/[controller]")]
    public class CLGAlumniDashboardController : Controller
    {
       
        CLGAlumniDashboardDelegate objdelegate = new CLGAlumniDashboardDelegate();

       
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("getalldetails")]
        public CLGAlumniStudentDTO getalldetails(CLGAlumniStudentDTO data)
        {
           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate.getalldetails(data);
        }

        [Route("saveakpkfile")]
        public CLGAlumniStudentDTO saveakpkfile([FromBody]CLGAlumniStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdelegate.saveakpkfile(data);
        }




    }
}

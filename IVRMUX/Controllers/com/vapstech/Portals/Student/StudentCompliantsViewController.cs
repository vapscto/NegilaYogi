using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Student
{
    [Route("api/[controller]")]
    public class StudentCompliantsViewController : Controller
    {
        StudentCompliantsViewDelegate del = new StudentCompliantsViewDelegate();
        [Route("loaddata/{id:int}")]
        public StudentCompliantsView_DTO loaddata(int id)
        {
            StudentCompliantsView_DTO data = new StudentCompliantsView_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
       
            return del.loaddata(data);
        }
        [Route("report1")]
        public StudentCompliantsView_DTO report1([FromBody] StudentCompliantsView_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return del.report1(data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using corewebapi18072016.Delegates.com.vapstech.Portals.Principal;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Principal
{
    [Route("api/[controller]")]
    public class StudentSearchController : Controller
    {
        StudentSearchDelegate objdelegate = new StudentSearchDelegate();

        //GET: api/values
       [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public StudentSearchDTO getdetails(StudentSearchDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.getalldetails(data);
        }

        //[Route("getstudentdetails/{Id:int}")]
        //public EmployeeDashboardDTO getstudentdetails(int Id)
        //{
        //    EmployeeDashboardDTO data = new EmployeeDashboardDTO();
        //    data.Amst_Id = Id;
        //    int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.MI_Id = mid;

        //    int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
        //    data.ASMAY_Id = ASMAY_Id;

        //    int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    data.userid = UserId;

        //    data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

        //    return objdelegate.getstudentdetails(data);
        //}
        [HttpPost]
        [Route("getstudentdetails")]
        public StudentSearchDTO getstudentdetails([FromBody]StudentSearchDTO data)
        
{
           
            //data.Amst_Id = Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

           // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
           // data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

           // data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.getstudentdetails(data);
        }

        [HttpPost]
        [Route("GetStudentDetails1")]
        public StudentSearchDTO GetStudentDetails1([FromBody]StudentSearchDTO data)        
{      
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return objdelegate.GetStudentDetails1(data);
        }
        [HttpPost]
        [Route("showsectionGrid")]
        public StudentSearchDTO showsectionGrid([FromBody]StudentSearchDTO data)        
{
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return objdelegate.showsectionGrid(data);
        }
    }
}

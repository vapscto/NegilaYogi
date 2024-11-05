using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.HOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.HOD;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.HOD
{
    [Route("api/[controller]")]
    public class HODExamTopperController : Controller
    {
        HODExamTopperDelegate crStr = new HODExamTopperDelegate();


        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        
        [HttpGet]
        [Route("Getdetails")]
        public HODExamTopper_DTO Getdetails(HODExamTopper_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getdetails(data);
        }


        [HttpGet]
        [Route("getcategory/{id}")]
        public HODExamTopper_DTO getcategory(int id)
        {
            HODExamTopper_DTO data = new HODExamTopper_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = id;

            return crStr.getcategory(data);
        }
        [HttpPost]
        [Route("getclassexam")]
        public HODExamTopper_DTO getclassexam([FromBody] HODExamTopper_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.getclassexam(data);

        }


        [HttpPost]
        [Route("showreport")]
        public HODExamTopper_DTO showreport([FromBody] HODExamTopper_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return crStr.showreport(data);
        }

        [HttpPost]
        [Route("getsection")]
        public HODExamTopper_DTO getsection([FromBody] HODExamTopper_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return crStr.getsection(data);

        }
    }
}

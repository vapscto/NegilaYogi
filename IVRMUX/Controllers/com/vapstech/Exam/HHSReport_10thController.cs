
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    public class HHSReport_10thController : Controller
    {


        HHSReport_10thDelegates crStr = new HHSReport_10thDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
      
        [HttpGet]
        [Route("Getdetails")]
        public HHSReport_10thDTO Getdetails(HHSReport_10thDTO data)
         {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.MI_Id = 5;
            return crStr.Getdetails(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public HHSReport_10thDTO savedetails([FromBody] HHSReport_10thDTO data)
        {
            // data.MI_Id = 5;
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.savedetails(data);

        }

        [Route("getclass/{id}")]
        public HHSReport_10thDTO getclass(int id)
        {
            HHSReport_10thDTO data = new HHSReport_10thDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            //data.MI_Id = 5;
            return crStr.getclass(data);
        }
        [HttpPost]
        [Route("Getsection")]
        public HHSReport_10thDTO Getsection([FromBody] HHSReport_10thDTO data)
        {
            // data.MI_Id = 5;
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getsection(data);

        }

        [HttpPost]
        [Route("GetAttendence")]
        public HHSReport_10thDTO GetAttendence([FromBody] HHSReport_10thDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.MI_Id = 5;
            return crStr.GetAttendence(data);

        }
        //[HttpPost]
        //[Route("GetIndividualAttendence")]
        //public HHSReport_10thDTO GetIndividualAttendence([FromBody] HHSReport_10thDTO data)
        //{
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    // data.MI_Id = 5;
        //    return crStr.GetIndividualAttendence(data);

        //}

    }

}

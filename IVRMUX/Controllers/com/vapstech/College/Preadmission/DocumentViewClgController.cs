using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Preadmission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Preadmission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Preadmission
{
    [Route("api/[controller]")]
    public class DocumentViewClgController : Controller
    {
        DocumentViewClgDelegate objdelegate = new DocumentViewClgDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [HttpGet]
        [Route("getdetails/{id:int}")]
        public CollegePreadmissionstudnetDto Get(int id)
        {
            CollegePreadmissionstudnetDto data = new CollegePreadmissionstudnetDto();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(data);
        }

        [HttpPost]
        [Route("getclgstudata")]
        public CollegePreadmissionstudnetDto getclgstudata([FromBody] CollegePreadmissionstudnetDto data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return objdelegate.getclgstudata(data);
        }
        [Route("getdocksonly")]
        public CollegePreadmissionstudnetDto getdocksonly([FromBody] CollegePreadmissionstudnetDto data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return objdelegate.getdocksonly(data);
        }
        //Admssion Register report college
        [HttpPost]
        [Route("Getregdata/")]
        public CollegePreadmissionstudnetDto Getregdata([FromBody] CollegePreadmissionstudnetDto MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;
            return objdelegate.GetData(MMD);
        }

        [Route("getbranch")]
        public CollegePreadmissionstudnetDto getbranch([FromBody]CollegePreadmissionstudnetDto data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
         
            return objdelegate.getbranch(data);
        }
        [Route("getsemester")]
        public CollegePreadmissionstudnetDto getsemester([FromBody]CollegePreadmissionstudnetDto data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
         
            return objdelegate.getsemester(data);
        }
    }
}

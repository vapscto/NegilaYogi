using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Sports;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportHouseReportController : Controller
    {
        SportHouseReportDelegate crStr = new SportHouseReportDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public House_Report_DTO Getdetails(House_Report_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }


        [Route("showdetails")]
        public House_Report_DTO showdetails([FromBody] House_Report_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return crStr.showdetails(data);
        }
        //showdetailsNew
        [Route("showdetailsNew")]
        public House_Report_DTO showdetailsNew([FromBody] House_Report_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return crStr.showdetailsNew(data);
        }
        [Route("get_class")]
        public House_Report_DTO get_class([FromBody]House_Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            

            return crStr.get_class(data);
        }

        [Route("get_section")]
        public House_Report_DTO get_section([FromBody]House_Report_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return crStr.get_section(data);
        }

        [Route("get_student")]
        public House_Report_DTO get_student([FromBody]House_Report_DTO data)

        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return crStr.get_student(data);
        }
    }
}

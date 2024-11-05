
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
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CumulativeReportController : Controller
    {


      CumulativeReportDelegates crStr = new CumulativeReportDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public CumulativeReportDTO Getdetails(CumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public CumulativeReportDTO editdetails(int ID)
        {
            return crStr.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public CumulativeReportDTO validateordernumber([FromBody] CumulativeReportDTO data)
        {
          //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.validateordernumber(data);
        }

        [Route("savedetails")]
        public CumulativeReportDTO savedetails([FromBody] CumulativeReportDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.savedetails(data);
        }

        [Route("deactivate")]
        public CumulativeReportDTO deactivate([FromBody] CumulativeReportDTO data)
        {
            return crStr.deactivate(data);         
        }

        [Route("onchangeyear")]
        public CumulativeReportDTO onchangeyear([FromBody] CumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public CumulativeReportDTO onchangeclass([FromBody] CumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangeclass(data);
        }

        [Route("onchangesection")]
        public CumulativeReportDTO onchangesection([FromBody] CumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangesection(data);
        }
    }
}

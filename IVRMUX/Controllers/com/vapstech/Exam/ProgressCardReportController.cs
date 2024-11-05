

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
    public class ProgressCardReportController : Controller
    {

      ProgressCardReportDelegates crStr = new ProgressCardReportDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public ProgressCardReportDTO Getdetails(ProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public ProgressCardReportDTO editdetails(int ID)
        {
            return crStr.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public ProgressCardReportDTO validateordernumber([FromBody] ProgressCardReportDTO data)
        {
          //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.validateordernumber(data);
        }

        [Route("savedetails")]
        public ProgressCardReportDTO savedetails([FromBody] ProgressCardReportDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.savedetails(data);
        }

        [Route("deactivate")]
        public ProgressCardReportDTO deactivate([FromBody] ProgressCardReportDTO data)
        {
            return crStr.deactivate(data);         
        }

       
    }

}

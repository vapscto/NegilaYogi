
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
    public class HHSMIDFINALCumReportController : Controller
    {


        HHSMIDFINALCumReportDelegates crStr = new HHSMIDFINALCumReportDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public HHSMIDFINALCumReportDTO Getdetails(HHSMIDFINALCumReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public HHSMIDFINALCumReportDTO editdetails(int ID)
        {
            return crStr.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public HHSMIDFINALCumReportDTO validateordernumber([FromBody] HHSMIDFINALCumReportDTO data)
        { 
            return crStr.validateordernumber(data);
        }

        [Route("savedetails")]
        public HHSMIDFINALCumReportDTO savedetails([FromBody] HHSMIDFINALCumReportDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.savedetails(data);
        }

        [Route("deactivate")]
        public HHSMIDFINALCumReportDTO deactivate([FromBody] HHSMIDFINALCumReportDTO data)
        {
            return crStr.deactivate(data);
        }
        [Route("savedetailsnew")]
        public HHSMIDFINALCumReportDTO savedetailsnew([FromBody] HHSMIDFINALCumReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.savedetailsnew(data);
        }

        [Route("cumulativereport")]
        public HHSMIDFINALCumReportDTO cumulativereport([FromBody] HHSMIDFINALCumReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.cumulativereport(data);
        }

        [Route("ExamSubExamCumulativeReport")]
        public HHSMIDFINALCumReportDTO ExamSubExamCumulativeReport([FromBody] HHSMIDFINALCumReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.ExamSubExamCumulativeReport(data);
        }        
    }
}

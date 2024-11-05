
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
    public class MarksEntry_Ent_ReportController : Controller
    {


        MarksEntry_Ent_ReportDelegates crStr = new MarksEntry_Ent_ReportDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails/{id:int}")]
        public MarksEntry_Ent_ReportDTO Getdetails(int id)
        {
            MarksEntry_Ent_ReportDTO data = new MarksEntry_Ent_ReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);           
        }

        [HttpPost]
        [Route("get_report")]
        public MarksEntry_Ent_ReportDTO get_report([FromBody] MarksEntry_Ent_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.get_report(data);
        }
        //SubjectList
        [Route("SubjectList")]
        public MarksEntry_Ent_ReportDTO SubjectList([FromBody] MarksEntry_Ent_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.SubjectList(data);
        }

    }
}

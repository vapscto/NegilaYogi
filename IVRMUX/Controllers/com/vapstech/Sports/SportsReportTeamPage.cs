using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportsReportTeamPage : Controller
    {
        SportsReportTeamPageDelegate crStr = new SportsReportTeamPageDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public SportsReportTeamPageDto Getdetails(SportsReportTeamPageDto data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }

        [Route("saveRecord")]
        public SportsReportTeamPageDto saveRecord([FromBody]SportsReportTeamPageDto data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.saveRecord(data);
        }

        [Route("showdetails")]
        public SportsReportTeamPageDto showdetails([FromBody] SportsReportTeamPageDto data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return crStr.showdetails(data);
        }


        [Route("get_modeldata")]
        public SportsReportTeamPageDto get_modeldata([FromBody] SportsReportTeamPageDto data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_modeldata(data);
        }

        [Route("get_student")]
        public SportsReportTeamPageDto get_student([FromBody]SportsReportTeamPageDto data)

        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_student(data);
        }


        [Route("EditRecord")]
        public SportsReportTeamPageDto EditRecord([FromBody]SportsReportTeamPageDto data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return crStr.EditRecord(data);
        }

        [Route("deactivate")]
        public SportsReportTeamPageDto deactivate([FromBody] SportsReportTeamPageDto d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.deactivate(d);
        }

        [Route("SaveRecords")]
        public SportsReportTeamPageDto SaveRecords([FromBody]SportsReportTeamPageDto data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.SaveRecords(data);
        }


        [Route("GetEditData")]
        public SportsReportTeamPageDto GetEditData([FromBody]SportsReportTeamPageDto data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return crStr.GetEditData(data);
        }

        [Route("deactivated")]
        public SportsReportTeamPageDto deactivated([FromBody] SportsReportTeamPageDto data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.deactivated(data);
        }

    }
}

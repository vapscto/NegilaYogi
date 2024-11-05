using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Birthday;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.BirthDay;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Birthday
{
    [Route("api/[controller]")]
    public class BirthDayGraphs : Controller
    {
        BirthDayGraphsDelegate dele = new BirthDayGraphsDelegate();
        // GET: api/values
        [HttpGet("{id:int}")]
        public BirthDayDTO GetData(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getdata(id);
        }           
        //----
        [HttpPost]
        [Route("getS")]
        public BirthDayDTO BirthDaystudentlist([FromBody]BirthDayDTO student)
        {
            student.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getlistthree(student);
        }
        [Route("BindStaff")]
        public BirthDayDTO BirthDaysatfflist([FromBody]BirthDayDTO staff)
        {
            staff.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.staflist(staff);
        }

        [Route("Sendsms")]
        public BirthDayDTO SmsSending([FromBody]BirthDayDTO sms)
        {
            sms.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.staflist1(sms);
        }
        [Route("Sendmsg")]
        public BirthDayDTO Sendmsg([FromBody]BirthDayDTO smsmsg)
        {
            smsmsg.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.Sendmsg(smsmsg);
        }
        [Route("getReport")]
        public BirthDayDTO getReport([FromBody] BirthDayDTO rpt)
        {
            rpt.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getReport(rpt);
        }

        [Route("getEmailSMSCount")]
        public BirthDayDTO getEmailSMSCount([FromBody] BirthDayDTO rpt)
        {
            rpt.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getEmailSMSCount(rpt);
        }
        [Route("SearchByColumn")]
        public BirthDayDTO SearchByColumn([FromBody] BirthDayDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.SearchByColumn(data);
        }

        [Route("getmonthreport")]
        public BirthDayDTO getmonthreport([FromBody] BirthDayDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getmonthreport(data);
        }

        [Route("getdetails")]
        public BirthDayDTO getdetails([FromBody]BirthDayDTO data)
        {

            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getdetails(data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using PreadmissionDTOs.com.vaps.FrontOffice;
//using corewebapi18072016.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.BirthDay;
using PreadmissionDTOs.com.vaps.BirthDay;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.FrontOffice
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class BirthDayController : Controller
    {
        BirthDayDelegate dele = new BirthDayDelegate();
        // GET: api/values
        [HttpGet("{id:int}")]
        public BirthDayDTO GetData(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getdata(id);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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

        [Route("whatsapp")]
        public BirthDayDTO whatsapp([FromBody] BirthDayDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.Whatsapp(data);
        }

    }
}

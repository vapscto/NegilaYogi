using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.com.vaps.admission.Delegates;
using PreadmissionDTOs;
using System.IO;
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;

namespace corewebapi18072016.com.vaps.admission.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SMSEmailSettingController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        SMSEmailSettingDelegate ad = new SMSEmailSettingDelegate();
        SMSEmailSettingDTO sms = new SMSEmailSettingDTO();

        public SMSEmailSettingController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/SMSEmailSetting
        [Route("getalldetails/{id:int}")]
        public SMSEmailSettingDTO Get([FromQuery] int id)
        {
            var mI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            sms.MI_Id = mI_Id;
            return ad.getSmsEmailSetting(sms);
        }
      
        [Route("getmodulePage/{id:int}")]
        public SMSEmailSettingDTO getmodulePage(int id)
        {
            sms.IVRMIM_Id = id;
            return ad.getmodulePage(sms);
        }
        [HttpPost]
        public SMSEmailSettingDTO Post([FromBody]SMSEmailSettingDTO ac)
        {
            var mI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            ac.MI_Id = mI_Id;
            return ad.getHeader(ac);
        }




        [Route("save")]
        public SmsEmailDTO save([FromBody] SmsEmailDTO smsDto)
        {
            var mI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            smsDto.MI_Id = mI_Id;
            smsDto.IVRMSTAUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.saveSmsEmailSettings(smsDto);
        }

        // POST: api/SMSEmailSetting
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT: api/SMSEmailSetting/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [Route("getdetails/{id:int}")]
        public SmsEmailDTO getdetail(int id)
        {
            HttpContext.Session.SetString("isesId", id.ToString()); //Set
            return ad.editDetails(id);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public SmsEmailDTO Delete(int id)
        {
            return ad.deleterec(id);
        }
        [Route("getParameter/{id:int}")]
        public SMS_MAIL_PARAMETER_DTO getParameter(int id)
        {
           return ad.parameter(id);
        }

        [Route("activedeactivesms")]
        public SmsEmailDTO activedeactivesms([FromBody] SmsEmailDTO data)
        {
            return ad.activedeactivesms(data);
        }
        [Route("activedeactiveemail")]
        public SmsEmailDTO activedeactiveemail([FromBody] SmsEmailDTO data)
        {
            return ad.activedeactiveemail(data);
        }

        [Route("viewtempate")]
        public SmsEmailDTO viewtempate([FromBody] SmsEmailDTO data)
        {
            return ad.viewtempate(data);
        }

        
    }
}

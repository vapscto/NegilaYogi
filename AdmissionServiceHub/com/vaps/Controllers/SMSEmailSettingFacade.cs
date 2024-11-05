using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.com.vaps.admission.Controllers
{
    [Route("api/[controller]")]
    public class SMSEmailSettingFacade : Controller
    {
        public SmsEmailSettingInterface _acd;
        public SMSEmailSettingFacade(SmsEmailSettingInterface acdm)
        {
            _acd = acdm;
        }

        // GET: api/values
        [Route("getSmsEmailSetting")]
        public SMSEmailSettingDTO Get([FromBody]SMSEmailSettingDTO sms)
        {
            return _acd.getallDetails(sms);
        }
        [Route("getmodulePage")]
        public SMSEmailSettingDTO getmodulePage([FromBody]SMSEmailSettingDTO sms)
        {
            return _acd.getmodulePage(sms);
        }
       
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public SMSEmailSettingDTO Post([FromBody]SMSEmailSettingDTO acdm)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _acd.getHeader(acdm);
            // return det;
        }
        [Route("saveEmailSetting")]
        public SmsEmailDTO saveSmsEmailSetting([FromBody]SmsEmailDTO acdm)
        {
            return _acd.save(acdm);
        }
        [Route("getdetails/{id:int}")]
        public SmsEmailDTO getDetails(int id)
        {
            // id = 12;
            return _acd.getdetails(id);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public SmsEmailDTO Deleterec(int id)
        {
            return _acd.deleterec(id);
        }
        [Route("parameter/{id:int}")]
        public SMS_MAIL_PARAMETER_DTO getParameter(int id)
        {
            // id = 12;
            return _acd.getParameter(id);
        }
        [Route("activedeactivesms")]
        public SmsEmailDTO activedeactivesms([FromBody]SmsEmailDTO data)
        {
            return _acd.activedeactivesms(data); 
        }
        [Route("activedeactiveemail")]
        public SmsEmailDTO activedeactiveemail([FromBody]SmsEmailDTO data)
        {
            return _acd.activedeactiveemail(data);
        }
        [Route("viewtempate")]
        public SmsEmailDTO viewtempate([FromBody]SmsEmailDTO data)
        {
            return _acd.viewtempate(data);
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.Transport;
using TransportServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class SMSEmailSendFacade : Controller
    {
        public SMSEmailSendInterface _feegrouppagee;
        // GET: api/values
        public SMSEmailSendFacade(SMSEmailSendInterface maspag)
        {
            _feegrouppagee = maspag;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        [Route("getdata/{id:int}")]
        public SMSEmailSendDTO getdataaa(int id)
        {
            return _feegrouppagee.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPost]

        [Route("Getreportdetails")]
        public SMSEmailSendDTO Getreportdetails([FromBody]SMSEmailSendDTO data)
        {
            return _feegrouppagee.Getreportdetails(data);
        }

        [Route("Whatsapp")]
        public async Task<SMSEmailSendDTO> Whatsapp([FromBody]SMSEmailSendDTO data)
        {
            return await _feegrouppagee.sendWhatsAppCall(data);
        }



        [HttpPost]

        [Route("smssend")]
        public Task<SMSEmailSendDTO> smssend([FromBody]SMSEmailSendDTO data)
        {
            return _feegrouppagee.smssend(data);
        }
        

              [HttpPost]

        [Route("emailsend")]
        public SMSEmailSendDTO emailsend([FromBody]SMSEmailSendDTO data)
        {
            return _feegrouppagee.emailsend(data);
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

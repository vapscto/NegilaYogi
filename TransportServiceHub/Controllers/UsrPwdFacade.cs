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
    public class UsrPwdFacade : Controller
    {
        public UsrPwdInterface _feegrouppagee;

        public UsrPwdFacade(UsrPwdInterface maspag)
        {
            _feegrouppagee = maspag;
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
        [HttpPost]

        [Route("createusrnme")]
        public SMSEmailSendDTO crete([FromBody]SMSEmailSendDTO data)
        {
            return _feegrouppagee.creusrnme(data);
        }


        [HttpPost]

        [Route("emailsend")]
        public SMSEmailSendDTO emailsend([FromBody]SMSEmailSendDTO data)
        {
            return _feegrouppagee.emailsend(data);
        }

    }
}

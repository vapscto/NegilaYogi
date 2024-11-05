using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SMSMail_HeaderFacade : Controller
    {
        public SMSMail_HeaderInterface _org;

        public SMSMail_HeaderFacade(SMSMail_HeaderInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public SMSMail_HeaderDTO getalldetails([FromBody] SMSMail_HeaderDTO data)
        {
            return _org.getalldetails(data);
        }
        [Route("getdata")]
        public SMSMail_HeaderDTO getdata([FromBody] SMSMail_HeaderDTO data)
        {
            return _org.getdata(data);
        }
        [Route("edittab1")]
        public SMSMail_HeaderDTO edittab1([FromBody]  SMSMail_HeaderDTO data)
        {
            return _org.edittab1(data);
        }
        [Route("delete")]
        public SMSMail_HeaderDTO delete([FromBody]  SMSMail_HeaderDTO data)
        {
            return _org.delete(data);
        }
      
        
    }
}

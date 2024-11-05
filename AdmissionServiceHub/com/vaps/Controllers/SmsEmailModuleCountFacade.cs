using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SmsEmailModuleCountFacade : Controller
    {
        public SmsEmailModuleCountInterface _feegrouppagee;
        // GET: api/values

        public SmsEmailModuleCountFacade(SmsEmailModuleCountInterface maspag)
        {
            _feegrouppagee = maspag;
        }
       
        [Route("getdetails")]
        public SmsEmailModuleCountDTO getdetails([FromBody]SmsEmailModuleCountDTO data)
        {
            return _feegrouppagee.getdetails(data);
        }
        [HttpPost]
        [Route("Getreportdetails")]
        public SmsEmailModuleCountDTO Getreportdetails([FromBody]SmsEmailModuleCountDTO data)
        {
            return _feegrouppagee.Getreportdetails(data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Controllers
{
    [Produces("application/json")]
    [Route("api/SmsEmailReportFacade")]
    public class SmsEmailReportFacadeController : Controller
    {
        public SmsEmailReportInterFace _ads;

        public SmsEmailReportFacadeController(SmsEmailReportInterFace adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public SmsEmailReportDTO getloaddata([FromBody]SmsEmailReportDTO data)
        {
            return _ads.getloaddata(data);
        }

        [HttpPost]
        [Route("getdata")]
        public SmsEmailReportDTO getdata([FromBody]SmsEmailReportDTO sddto)
        {
            return _ads.getdata(sddto);
        }

    }
}
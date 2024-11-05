using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HRResumeUploadFacadeController : Controller
    {

        public HRResumeUploadInterface _ads;

        public HRResumeUploadFacadeController(HRResumeUploadInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        public HR_Resume_UploadDTO Post([FromBody]HR_Resume_UploadDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("AdmissionEnquirymail")]
        public HR_Resume_UploadDTO AdmissionEnquirymail([FromBody]HR_Resume_UploadDTO dto)
        {
            return _ads.AdmissionEnquirymail(dto);
        }
    }
}

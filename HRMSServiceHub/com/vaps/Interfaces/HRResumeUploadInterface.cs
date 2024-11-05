using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface HRResumeUploadInterface
    {
        HR_Resume_UploadDTO SaveUpdate(HR_Resume_UploadDTO dto);
        HR_Resume_UploadDTO AdmissionEnquirymail(HR_Resume_UploadDTO dto);
    }
}

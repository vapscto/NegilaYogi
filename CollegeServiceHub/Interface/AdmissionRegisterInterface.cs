using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface AdmissionRegisterInterface
    {
        AdmissionRegisterDTO getdetails(AdmissionRegisterDTO data);
        AdmissionRegisterDTO onselectAcdYear(AdmissionRegisterDTO data);
        AdmissionRegisterDTO onselectCourse(AdmissionRegisterDTO data);
        AdmissionRegisterDTO onselectBranch(AdmissionRegisterDTO data);
        Task<AdmissionRegisterDTO> onreport(AdmissionRegisterDTO data);
        Task<AdmissionRegisterDTO> onreportnew(AdmissionRegisterDTO data);
        
    }
}

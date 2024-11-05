using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface statewisestudentadmissionInterface
    {
        statewisestudentadmissionDTO getdetails(statewisestudentadmissionDTO data);
        statewisestudentadmissionDTO onselectAcdYear(statewisestudentadmissionDTO data);
        statewisestudentadmissionDTO onselectCourse(statewisestudentadmissionDTO data);
        statewisestudentadmissionDTO onselectBranch(statewisestudentadmissionDTO data);
        Task<statewisestudentadmissionDTO> onreport(statewisestudentadmissionDTO data);
        Task<statewisestudentadmissionDTO> onreportcountry(statewisestudentadmissionDTO data);
        Task<statewisestudentadmissionDTO> onreportreligionruralurban(statewisestudentadmissionDTO data);
        Task<statewisestudentadmissionDTO> CategoryCasteWiseStudentReport(statewisestudentadmissionDTO data);
        Task<statewisestudentadmissionDTO> onreportbirthday(statewisestudentadmissionDTO data);
        
    }
}

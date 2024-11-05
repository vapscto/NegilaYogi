using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface TeresianReportInterface
    {
        TeresianReportDTO getdetails(TeresianReportDTO data);
        TeresianReportDTO onselectAcdYear(TeresianReportDTO data);
        TeresianReportDTO onselectCourse(TeresianReportDTO data);
        TeresianReportDTO onselectBranch(TeresianReportDTO data);
        Task<TeresianReportDTO> onreport(TeresianReportDTO data);
        TeresianReportDTO onselectcategory(TeresianReportDTO data);
        
    }
}

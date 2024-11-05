using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterCourseInterface
    {
        HR_Master_CourseDTO getBasicData(HR_Master_CourseDTO dto);
        HR_Master_CourseDTO changeorderData(HR_Master_CourseDTO dto);
        HR_Master_CourseDTO SaveUpdate(HR_Master_CourseDTO dto);
        HR_Master_CourseDTO editData(int id);
        HR_Master_CourseDTO deactivate(HR_Master_CourseDTO dto);

    }
}

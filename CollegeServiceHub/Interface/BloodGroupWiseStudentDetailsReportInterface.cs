using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
   public interface BloodGroupWiseStudentDetailsReportInterface
    {
        BloodGroupWiseStudentDetailsReportDTO loaddata(BloodGroupWiseStudentDetailsReportDTO data);
        BloodGroupWiseStudentDetailsReportDTO getcourse(BloodGroupWiseStudentDetailsReportDTO data);
        BloodGroupWiseStudentDetailsReportDTO getbranch(BloodGroupWiseStudentDetailsReportDTO data);
        BloodGroupWiseStudentDetailsReportDTO getsemester(BloodGroupWiseStudentDetailsReportDTO data);
        BloodGroupWiseStudentDetailsReportDTO Report(BloodGroupWiseStudentDetailsReportDTO data);
    }
}

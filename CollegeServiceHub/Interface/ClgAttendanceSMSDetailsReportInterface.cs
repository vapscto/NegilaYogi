using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
   public interface ClgAttendanceSMSDetailsReportInterface
    {
        ClgAttendanceSMSDetailsReport_DTO loaddata(ClgAttendanceSMSDetailsReport_DTO data);
        ClgAttendanceSMSDetailsReport_DTO getcourse(ClgAttendanceSMSDetailsReport_DTO data);
        ClgAttendanceSMSDetailsReport_DTO getbranch(ClgAttendanceSMSDetailsReport_DTO data);
        ClgAttendanceSMSDetailsReport_DTO getsemester(ClgAttendanceSMSDetailsReport_DTO data);
        Task<ClgAttendanceSMSDetailsReport_DTO> showdetails(ClgAttendanceSMSDetailsReport_DTO data);

    }
}

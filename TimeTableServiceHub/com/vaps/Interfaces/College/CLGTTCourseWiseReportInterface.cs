using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public  interface CLGTTCourseWiseReportInterface
    {
        CLGTTCourseWiseReportDTO getreport(CLGTTCourseWiseReportDTO data);
        CLGTTCourseWiseReportDTO getclass_catg(CLGTTCourseWiseReportDTO data);
        CLGTTCourseWiseReportDTO getdetails(CLGTTCourseWiseReportDTO data);
        Task<CLGTTCourseWiseReportDTO> GetStudentDetails(CLGTTCourseWiseReportDTO data);
    }
}

using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public  interface CLGTTCourseReportBWMCInterface
    {
        CLGTTCourseReportBWMCDTO getreport(CLGTTCourseReportBWMCDTO data);
        CLGTTCourseReportBWMCDTO getclass_catg(CLGTTCourseReportBWMCDTO data);
        CLGTTCourseReportBWMCDTO getdetails(CLGTTCourseReportBWMCDTO data);
      

    }
}

using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public  interface CLGTTStaffWiseReportInterface
    {
        CLGTTStaffWiseReportDTO getreport(CLGTTStaffWiseReportDTO data);
        CLGTTStaffWiseReportDTO getdetails(CLGTTStaffWiseReportDTO data);
        Task<CLGTTStaffWiseReportDTO> GetStaffDetails(CLGTTStaffWiseReportDTO data);
    }
}

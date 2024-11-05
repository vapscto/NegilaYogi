using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface SMSemailStaffReportInterface
    {
        SMSemailStaffReportDTO getBasicData(SMSemailStaffReportDTO dto);     
        SMSemailStaffReportDTO getreport(SMSemailStaffReportDTO dto);
        SMSemailStaffReportDTO smsemail(SMSemailStaffReportDTO dto);
        //Destination
        SMSemailStaffReportDTO Destination(SMSemailStaffReportDTO dto);

    }
}

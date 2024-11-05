using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public  interface CLGTTSubjectWiseReportInterface
    {

        CLGTTSubjectWiseReportDTO getdetails(CLGTTSubjectWiseReportDTO data);
        CLGTTSubjectWiseReportDTO getbranch(CLGTTSubjectWiseReportDTO data);
        CLGTTSubjectWiseReportDTO getsemister(CLGTTSubjectWiseReportDTO data);
        CLGTTSubjectWiseReportDTO savedetail(CLGTTSubjectWiseReportDTO data);
    }
}

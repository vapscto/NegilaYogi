using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
  public  interface AdmissionSMSReportInterface
    {
        AdmissionSMSReportDTO getdetails(AdmissionSMSReportDTO id);

        AdmissionSMSReportDTO Getreportdetails(AdmissionSMSReportDTO data);
    
    }
}

using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
 public   interface BBKVCustomReportInterface
    {
        BBKVCustomReportDTO getdetails(int id);
        BBKVCustomReportDTO getnameregno(BBKVCustomReportDTO data);
        BBKVCustomReportDTO stdnamechange(BBKVCustomReportDTO data);
        BBKVCustomReportDTO onclicktcperortemo(BBKVCustomReportDTO data);
        BBKVCustomReportDTO get_JSHSTcdetails(BBKVCustomReportDTO data);
        BBKVCustomReportDTO getTcdetails(BBKVCustomReportDTO data);
        BBKVCustomReportDTO getTcdetailsJNS(BBKVCustomReportDTO data);
    }
}

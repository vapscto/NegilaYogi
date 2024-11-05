using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface DocumentViewReportAdmInterface
    {
        DocumentViewReportAdmDTO getdetails(int id);
        DocumentViewReportAdmDTO getstudent(DocumentViewReportAdmDTO data);
        DocumentViewReportAdmDTO getreport(DocumentViewReportAdmDTO data);
    }
}

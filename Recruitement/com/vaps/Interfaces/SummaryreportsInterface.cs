using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface SummaryreportsInterface
    {
        SummaryreportsDTO onloaddata(SummaryreportsDTO data);
        SummaryreportsDTO getreport(SummaryreportsDTO data);
        SummaryreportsDTO inhouseReportreport(SummaryreportsDTO data);
        SummaryreportsDTO trainingcertificate(SummaryreportsDTO data);
    }
}

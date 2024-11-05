using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
    public interface NaacCriteria4ReportInterface
    {

        NaacCriteria4ReportDTO loaddata(NaacCriteria4ReportDTO data);
       Task<NaacCriteria4ReportDTO> Report(NaacCriteria4ReportDTO data);
        NaacCriteria4ReportDTO ReportCriteria4(NaacCriteria4ReportDTO data);
     
       Task<NaacCriteria4ReportDTO> ExpAcaReport(NaacCriteria4ReportDTO data);

    }
}

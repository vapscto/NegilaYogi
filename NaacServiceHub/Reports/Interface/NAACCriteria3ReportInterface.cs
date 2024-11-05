using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface NAACCriteria3ReportInterface
    {
        NAACCriteria3ReportDTO getdata(NAACCriteria3ReportDTO data);
        Task<NAACCriteria3ReportDTO> get_report(NAACCriteria3ReportDTO data);
        Task<NAACCriteria3ReportDTO> get_report364(NAACCriteria3ReportDTO data);
        Task<NAACCriteria3ReportDTO> reportIPR(NAACCriteria3ReportDTO data);
      

    }
}

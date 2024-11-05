using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeDueDateReportInterface
    {
        FeeDueDateReportDTO getInitailData(FeeDueDateReportDTO data);
        Task<FeeDueDateReportDTO> SearchData(FeeDueDateReportDTO Clscatag);
        FeeDueDateReportDTO getdata(FeeDueDateReportDTO data);
        //Income Report
        Task<FeeDueDateReportDTO> getreport(FeeDueDateReportDTO data);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeHeadWiseReportInterface
    {
        FeeHeadWiseReportDTO getInitailData(FeeHeadWiseReportDTO data);
        FeeHeadWiseReportDTO SearchData(FeeHeadWiseReportDTO Clscatag);

        FeeHeadWiseReportDTO getdata(FeeHeadWiseReportDTO data);

        // Sudarshan 02-12-2023
        FeeHeadWiseReportDTO getreport(FeeHeadWiseReportDTO data);
    }
}

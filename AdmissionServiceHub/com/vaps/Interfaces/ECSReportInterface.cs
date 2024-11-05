using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface ECSReportInterface
    {
        ECSReportDTO getloaddata(ECSReportDTO data);
        ECSReportDTO getclass(ECSReportDTO data);
        ECSReportDTO getsection(ECSReportDTO data);
        ECSReportDTO getreport(ECSReportDTO data);
        ECSReportDTO showecsdetails(ECSReportDTO data);
        ECSReportDTO searchByColumn(ECSReportDTO data);
    }
}

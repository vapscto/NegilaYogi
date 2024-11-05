using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.IVRM.Interfaces
{
    public interface PortalMonthEndReportInterface
    {
        PortalMonthEndReportDTO getloaddata(PortalMonthEndReportDTO data);
        Task<PortalMonthEndReportDTO> getmonthreport(PortalMonthEndReportDTO data);      

    }

}

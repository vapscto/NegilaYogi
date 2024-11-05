using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface LeftStudentsReportInterface
    {
        LeftStudentsReportDTO loaddata(LeftStudentsReportDTO data);
        LeftStudentsReportDTO getCategory(LeftStudentsReportDTO data);
        LeftStudentsReportDTO getClass(LeftStudentsReportDTO data);
        LeftStudentsReportDTO getsection(LeftStudentsReportDTO data);
        LeftStudentsReportDTO report(LeftStudentsReportDTO data);
    }
}

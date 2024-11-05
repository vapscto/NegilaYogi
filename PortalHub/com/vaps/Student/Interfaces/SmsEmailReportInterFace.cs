using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface SmsEmailReportInterFace
    {
        SmsEmailReportDTO getloaddata(SmsEmailReportDTO data);
        SmsEmailReportDTO getdata(SmsEmailReportDTO data);

    }
}

using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface PreadmissionNoticeRegistrationReportInterface
    {
        PreadmissionNoticeRegistrationReportDTO get_intial_data(PreadmissionNoticeRegistrationReportDTO data);
        PreadmissionNoticeRegistrationReportDTO getprospectusno(PreadmissionNoticeRegistrationReportDTO data);
        PreadmissionNoticeRegistrationReportDTO getstudentlist(PreadmissionNoticeRegistrationReportDTO data);
        PreadmissionNoticeRegistrationReportDTO addtolist(PreadmissionNoticeRegistrationReportDTO data);
        PreadmissionNoticeRegistrationReportDTO Savedata(PreadmissionNoticeRegistrationReportDTO data);
        PreadmissionNoticeRegistrationReportDTO viewstudent(PreadmissionNoticeRegistrationReportDTO data);
        PreadmissionNoticeRegistrationReportDTO Editdata(PreadmissionNoticeRegistrationReportDTO data);
        PreadmissionNoticeRegistrationReportDTO printData(PreadmissionNoticeRegistrationReportDTO data);
    }
}

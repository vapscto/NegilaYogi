using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ToppersListReportInterface
    {
        ToppersListReportDTO getdetails(ToppersListReportDTO data);
        ToppersListReportDTO onselectCategory(ToppersListReportDTO data);
        ToppersListReportDTO onselectclass(ToppersListReportDTO data);
        ToppersListReportDTO onreport(ToppersListReportDTO data);
        ToppersListReportDTO get_sec_exam(ToppersListReportDTO data);
        ToppersListReportDTO onselectexam(ToppersListReportDTO data);
        ToppersListReportDTO get_subject(ToppersListReportDTO data);
        ToppersListReportDTO sendsms(ToppersListReportDTO data);

        //Kiosk.
        ToppersListReportDTO.KioskExamTopperDTO kioskExamToppers(ToppersListReportDTO data);
    }
}

using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface MaldaProgressReportExamInterface
    {
        Task<MaldaProgressReportExam_DTO> Getdetails(MaldaProgressReportExam_DTO data);
        Task<MaldaProgressReportExam_DTO> savedetails(MaldaProgressReportExam_DTO data);

        MaldaProgressReportExam_DTO onchangeyear(MaldaProgressReportExam_DTO data);
        MaldaProgressReportExam_DTO onchangeclass(MaldaProgressReportExam_DTO data);
        MaldaProgressReportExam_DTO onchangesection(MaldaProgressReportExam_DTO data);
        MaldaProgressReportExam_DTO getreportpromotion(MaldaProgressReportExam_DTO data);
        MaldaProgressReportExam_DTO ixpromotionreport(MaldaProgressReportExam_DTO data);
        
    }
}

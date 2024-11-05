using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface ExamPromotionReportInterface
    {

        ExamPromotionReport_DTO Getdetails(ExamPromotionReport_DTO data);
        ExamPromotionReport_DTO get_class(ExamPromotionReport_DTO data);
        ExamPromotionReport_DTO get_section(ExamPromotionReport_DTO data);
        ExamPromotionReport_DTO get_exam(ExamPromotionReport_DTO data);
        Task<ExamPromotionReport_DTO> get_reports(ExamPromotionReport_DTO data);

    }
}

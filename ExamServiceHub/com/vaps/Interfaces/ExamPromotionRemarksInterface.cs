using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamPromotionRemarksInterface
    {
        ExamPromotionRemarksDTO Getdetails(ExamPromotionRemarksDTO data);
        ExamPromotionRemarksDTO get_class(ExamPromotionRemarksDTO data);
        ExamPromotionRemarksDTO get_section(ExamPromotionRemarksDTO data);
        ExamPromotionRemarksDTO get_group(ExamPromotionRemarksDTO data);
        ExamPromotionRemarksDTO get_exam(ExamPromotionRemarksDTO data);
        ExamPromotionRemarksDTO search_student(ExamPromotionRemarksDTO data);
        ExamPromotionRemarksDTO search_groupwise_student(ExamPromotionRemarksDTO data);
        ExamPromotionRemarksDTO save_details(ExamPromotionRemarksDTO data);
        ExamPromotionRemarksDTO save_groupwise_details(ExamPromotionRemarksDTO data);

    }
}

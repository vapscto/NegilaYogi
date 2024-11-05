using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface ExamMarksprocessconditionInterface
    {
        ExamMarksProcess_DTO Getdetails(ExamMarksProcess_DTO data);
        ExamMarksProcess_DTO get_category(ExamMarksProcess_DTO data);
        ExamMarksProcess_DTO get_subjects(ExamMarksProcess_DTO data);
        ExamMarksProcess_DTO savedetails(ExamMarksProcess_DTO data);
        ExamMarksProcess_DTO editdetails(ExamMarksProcess_DTO data);
        ExamMarksProcess_DTO deactivate(ExamMarksProcess_DTO data);
        ExamMarksProcess_DTO get_exm_details(ExamMarksProcess_DTO data);
        ExamMarksProcess_DTO getalldetailsviewrecords(ExamMarksProcess_DTO data);
        ExamMarksProcess_DTO saveUserPromotionData(ExamMarksProcess_DTO data);
        ExamMarksProcess_DTO deActiveUserPromotion(ExamMarksProcess_DTO data);
        ExamMarksProcess_DTO editUserPromotion(ExamMarksProcess_DTO data);
        //saveUserPromotionDataNew
        ExamMarksProcess_DTO saveUserPromotionDataNew(ExamMarksProcess_DTO data);
    }
}

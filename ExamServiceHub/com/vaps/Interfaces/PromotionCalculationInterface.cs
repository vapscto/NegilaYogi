
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface PromotionCalculationInterface
    {
        PromotionCalculationDTO Getdetails(PromotionCalculationDTO data);
        PromotionCalculationDTO get_cls_sections(PromotionCalculationDTO data);
        PromotionCalculationDTO Calculation(PromotionCalculationDTO data);
        PromotionCalculationDTO get_classes(PromotionCalculationDTO id);
        PromotionCalculationDTO publishtostudentportal(PromotionCalculationDTO id);
        PromotionCalculationDTO onchangesection(PromotionCalculationDTO id);
      
    }
}

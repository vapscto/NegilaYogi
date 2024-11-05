using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface Promotion_Marks_UpdateInterface
    {
        Promotion_Marks_UpdateDTO Getdetails(Promotion_Marks_UpdateDTO data);
        Promotion_Marks_UpdateDTO get_categories(Promotion_Marks_UpdateDTO data);
        Promotion_Marks_UpdateDTO get_classes(Promotion_Marks_UpdateDTO data);
        Promotion_Marks_UpdateDTO get_sections(Promotion_Marks_UpdateDTO data);
        Promotion_Marks_UpdateDTO get_subjects(Promotion_Marks_UpdateDTO data);
        Promotion_Marks_UpdateDTO get_prommarks(Promotion_Marks_UpdateDTO data);
    }
}

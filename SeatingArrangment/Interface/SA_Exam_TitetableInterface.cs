using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatingArrangment.Interface
{
    public interface SA_Exam_TitetableInterface
    {
        SA_Exam_TitetableDTO load_TT(SA_Exam_TitetableDTO dto);
        SA_Exam_TitetableDTO Save_TT(SA_Exam_TitetableDTO dto);
        SA_Exam_TitetableDTO Edit_TT(SA_Exam_TitetableDTO dto);
        SA_Exam_TitetableDTO Deactive_TT(SA_Exam_TitetableDTO dto);
        SA_Exam_TitetableDTO viewTTdetails(SA_Exam_TitetableDTO dto);
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamPassFailConditionInterface
    {
        ExamPassFailConditionDTO Getdetails(ExamPassFailConditionDTO data);
        ExamPassFailConditionDTO get_category(ExamPassFailConditionDTO data);
        ExamPassFailConditionDTO get_subjects(ExamPassFailConditionDTO data);    
          ExamPassFailConditionDTO get_examcondition(ExamPassFailConditionDTO data);
        ExamPassFailConditionDTO get_condition(ExamPassFailConditionDTO data);
        ExamPassFailConditionDTO deactive(ExamPassFailConditionDTO data);
        ExamPassFailConditionDTO editdetails(int ID);
        ExamPassFailConditionDTO savedata(ExamPassFailConditionDTO data);

    }
}

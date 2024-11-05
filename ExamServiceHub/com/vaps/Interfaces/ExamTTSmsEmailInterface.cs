using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamTTSmsEmailInterface
    {
        ExamTTSmsEmailDTO getdetails(ExamTTSmsEmailDTO data);
        ExamTTSmsEmailDTO onselectAcdYear(ExamTTSmsEmailDTO data);
        ExamTTSmsEmailDTO onselectclass(ExamTTSmsEmailDTO data);
        ExamTTSmsEmailDTO onselectSection(ExamTTSmsEmailDTO data);
        ExamTTSmsEmailDTO getStudentsTeachers(ExamTTSmsEmailDTO data);
        ExamTTSmsEmailDTO generate(ExamTTSmsEmailDTO data);
        ExamTTSmsEmailDTO sendmail(ExamTTSmsEmailDTO data);
        ExamTTSmsEmailDTO getsubjectgroup(ExamTTSmsEmailDTO data);
        
    }
}

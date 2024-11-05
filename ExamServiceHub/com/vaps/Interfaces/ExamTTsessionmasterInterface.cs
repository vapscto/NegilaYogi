using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamTTsessionmasterInterface
    {
        ExamTTsessionmasterDTO Getdetails(ExamTTsessionmasterDTO data);
        ExamTTsessionmasterDTO savedetails(ExamTTsessionmasterDTO data);
        ExamTTsessionmasterDTO editdetails(ExamTTsessionmasterDTO data);
        ExamTTsessionmasterDTO deactivate(ExamTTsessionmasterDTO data);
        
    }


}

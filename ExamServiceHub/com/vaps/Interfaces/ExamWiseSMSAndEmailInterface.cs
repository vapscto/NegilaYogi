using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamWiseSMSAndEmailInterface
    {
        ExamWiseSMSAndEmailDTO loaddata(ExamWiseSMSAndEmailDTO data);
        ExamWiseSMSAndEmailDTO getclass(ExamWiseSMSAndEmailDTO data);
        ExamWiseSMSAndEmailDTO getsection(ExamWiseSMSAndEmailDTO data);
        ExamWiseSMSAndEmailDTO getexam(ExamWiseSMSAndEmailDTO data);
        ExamWiseSMSAndEmailDTO searchDetails(ExamWiseSMSAndEmailDTO data);
        ExamWiseSMSAndEmailDTO SendSmsEmail(ExamWiseSMSAndEmailDTO data);
    }
}

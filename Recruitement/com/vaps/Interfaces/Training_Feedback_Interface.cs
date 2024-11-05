using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Training_Feedback_Interface
    {
        //---------------------- building-----------------------
        HR_Training_Feedback_DTO loaddata(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO getQuestions(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO savetrainerfeedback(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO savetraineefeedback(HR_Training_Feedback_DTO dto);
    }
}
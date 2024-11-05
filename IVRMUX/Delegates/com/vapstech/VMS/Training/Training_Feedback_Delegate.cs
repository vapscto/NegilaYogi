using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class Training_Feedback_Delegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonVMSDelegate<HR_Training_Feedback_DTO, HR_Training_Feedback_DTO> COMMB = new CommonVMSDelegate<HR_Training_Feedback_DTO, HR_Training_Feedback_DTO>();
       
        public HR_Training_Feedback_DTO loaddata(HR_Training_Feedback_DTO id)
        {
            return COMMB.POSTData(id, "Training_FeedbackFacade/loaddata/");
        }
        public HR_Training_Feedback_DTO getQuestions(HR_Training_Feedback_DTO dto)
        {
            return COMMB.POSTData(dto, "Training_FeedbackFacade/getQuestions/");
        }
        public HR_Training_Feedback_DTO savetrainerfeedback(HR_Training_Feedback_DTO dto)
        {
            return COMMB.POSTData(dto, "Training_FeedbackFacade/savetrainerfeedback/");
        }

        public HR_Training_Feedback_DTO savetraineefeedback(HR_Training_Feedback_DTO dto)
        {
            return COMMB.POSTData(dto, "Training_FeedbackFacade/savetraineefeedback/");
        }
        
    }
}


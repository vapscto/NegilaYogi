using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Controllers.com.vapstech.VMS.Training
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class Training_FeedbackController : Controller
    {
        Training_Feedback_Delegate TMD = new Training_Feedback_Delegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public HR_Training_Feedback_DTO loaddata(int id)
        {
            HR_Training_Feedback_DTO dto = new HR_Training_Feedback_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.loaddata(dto);
        }
        [Route("getQuestions")]
        public HR_Training_Feedback_DTO getQuestions([FromBody] HR_Training_Feedback_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.getQuestions(dto);
        }

        [HttpPost]
        [Route("savetrainerfeedback")]
        public HR_Training_Feedback_DTO savetrainerfeedback([FromBody] HR_Training_Feedback_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRTFEED_ActiveFlg = dto.HRTFEED_ActiveFlg;
            return TMD.savetrainerfeedback(dto);
        }

        [HttpPost]
        [Route("savetraineefeedback")]
        public HR_Training_Feedback_DTO savetraineefeedback([FromBody] HR_Training_Feedback_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRTFEED_ActiveFlg = dto.HRTFEED_ActiveFlg;
            return TMD.savetraineefeedback(dto);
        }

    }
}
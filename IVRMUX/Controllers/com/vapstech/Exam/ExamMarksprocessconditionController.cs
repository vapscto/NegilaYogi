using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class ExamMarksprocessconditionController : Controller
    {
        ExamMarksprocessconditionDelegate _ExamMarksProcess = new ExamMarksprocessconditionDelegate();

        [Route("Getdetails")]
        public ExamMarksProcess_DTO Getdetails(ExamMarksProcess_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _ExamMarksProcess.Getdetails(data);
           
        }

        [Route("get_category")]
        public ExamMarksProcess_DTO get_category([FromBody] ExamMarksProcess_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _ExamMarksProcess.get_category(data);
        }

        [Route("get_subjects")]
        public ExamMarksProcess_DTO get_subjects([FromBody] ExamMarksProcess_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _ExamMarksProcess.get_subjects(data);
        }
       
        [Route("savedetails")]
        public ExamMarksProcess_DTO savedetails([FromBody] ExamMarksProcess_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _ExamMarksProcess.savedetails(data);
        }

        [Route("editdetails")]
        public ExamMarksProcess_DTO editdetails([FromBody] ExamMarksProcess_DTO data)
        {
           // ExamMarksProcess_DTO data = new ExamMarksProcess_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _ExamMarksProcess.editdetails(data);

        }

        [Route("deactivate")]
        public ExamMarksProcess_DTO deactivate([FromBody] ExamMarksProcess_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _ExamMarksProcess.deactivate(data);
        }

        [Route("get_exm_details")]
        public ExamMarksProcess_DTO get_exm_details([FromBody] ExamMarksProcess_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _ExamMarksProcess.get_exm_details(data);
        }
        
        [Route("getalldetailsviewrecords")]
        public ExamMarksProcess_DTO getalldetailsviewrecords([FromBody] ExamMarksProcess_DTO data)
        {
            //ExamMarksProcess_DTO data = new ExamMarksProcess_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));        
            return _ExamMarksProcess.getalldetailsviewrecords(data);
        }

        // User Promotion
        [Route("saveUserPromotionData")]
        public ExamMarksProcess_DTO saveUserPromotionData([FromBody] ExamMarksProcess_DTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _ExamMarksProcess.saveUserPromotionData(data);
        }
        // saveUserPromotionDataNew
        [Route("saveUserPromotionDataNew")]
        public ExamMarksProcess_DTO saveUserPromotionDataNew([FromBody] ExamMarksProcess_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _ExamMarksProcess.saveUserPromotionDataNew(data);
        }
        [Route("deActiveUserPromotion")]
        public ExamMarksProcess_DTO deActiveUserPromotion([FromBody] ExamMarksProcess_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _ExamMarksProcess.deActiveUserPromotion(data);
        }

        [Route("editUserPromotion")]
        public ExamMarksProcess_DTO editUserPromotion([FromBody] ExamMarksProcess_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _ExamMarksProcess.editUserPromotion(data);
        }
        
    }
}

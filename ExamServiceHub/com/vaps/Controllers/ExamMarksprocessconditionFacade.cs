using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamMarksprocessconditionFacade : Controller
    {

        public ExamMarksprocessconditionInterface _IExamMarksProcess;

        public ExamMarksprocessconditionFacade(ExamMarksprocessconditionInterface ExamSubjectMapping)
        {
            _IExamMarksProcess = ExamSubjectMapping;
        }


        [Route("Getdetails")]
        public ExamMarksProcess_DTO Getdetails([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.Getdetails(data);
        }

        [Route("get_category")]
        public ExamMarksProcess_DTO get_category([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.get_category(data);
        }

        [Route("get_subjects")]
        public ExamMarksProcess_DTO get_subjects([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.get_subjects(data);
        }

        [Route("savedetails")]
        public ExamMarksProcess_DTO savedetails([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.savedetails(data);
        }

        [Route("editdetails")]
        public ExamMarksProcess_DTO editdetails([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.editdetails(data);
        }

        [Route("deactivate")]
        public ExamMarksProcess_DTO deactivate([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.deactivate(data);
        }
        [Route("get_exm_details")]
        public ExamMarksProcess_DTO get_exm_details([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.get_exm_details(data);
        }
        [Route("getalldetailsviewrecords")]
        public ExamMarksProcess_DTO getalldetailsviewrecords([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.getalldetailsviewrecords(data);
        }

        //User Promotion
        [Route("saveUserPromotionData")]
        public ExamMarksProcess_DTO saveUserPromotionData([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.saveUserPromotionData(data);
        }
        //saveUserPromotionDataNew
        [Route("saveUserPromotionDataNew")]
        public ExamMarksProcess_DTO saveUserPromotionDataNew([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.saveUserPromotionDataNew(data);
        }
        [Route("deActiveUserPromotion")]
        public ExamMarksProcess_DTO deActiveUserPromotion([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.deActiveUserPromotion(data);
        }

        [Route("editUserPromotion")]
        public ExamMarksProcess_DTO editUserPromotion([FromBody]ExamMarksProcess_DTO data)
        {
            return _IExamMarksProcess.editUserPromotion(data);
        }
        
    }
}

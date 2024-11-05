using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class ExamMarksprocessconditionDelegate
    {
        CommonDelegate<ExamMarksProcess_DTO, ExamMarksProcess_DTO> COMMM1 = new CommonDelegate<ExamMarksProcess_DTO, ExamMarksProcess_DTO>();
        public ExamMarksProcess_DTO Getdetails(ExamMarksProcess_DTO id)
        {
            return COMMM1.POSTDataExam(id, "ExamMarksprocessconditionFacade/Getdetails/");
        }

        public ExamMarksProcess_DTO get_category(ExamMarksProcess_DTO data)
        {
            return COMMM1.POSTDataExam(data, "ExamMarksprocessconditionFacade/get_category/");
        }
        public ExamMarksProcess_DTO get_subjects(ExamMarksProcess_DTO data)
        {
            return COMMM1.POSTDataExam(data, "ExamMarksprocessconditionFacade/get_subjects/");
        }
        public ExamMarksProcess_DTO savedetails(ExamMarksProcess_DTO data)
        {
            return COMMM1.POSTDataExam(data, "ExamMarksprocessconditionFacade/savedetails/");
        }
        public ExamMarksProcess_DTO editdetails(ExamMarksProcess_DTO data)
        {
            return COMMM1.POSTDataExam(data, "ExamMarksprocessconditionFacade/editdetails/");
        }
        public ExamMarksProcess_DTO deactivate(ExamMarksProcess_DTO data)
        {
            return COMMM1.POSTDataExam(data, "ExamMarksprocessconditionFacade/deactivate/");
        }
        public ExamMarksProcess_DTO get_exm_details(ExamMarksProcess_DTO data)
        {
            return COMMM1.POSTDataExam(data, "ExamMarksprocessconditionFacade/get_exm_details/");
        }
        public ExamMarksProcess_DTO getalldetailsviewrecords(ExamMarksProcess_DTO data)
        {
            return COMMM1.POSTDataExam(data, "ExamMarksprocessconditionFacade/getalldetailsviewrecords/");
        }
        // User Promotion
        public ExamMarksProcess_DTO saveUserPromotionData(ExamMarksProcess_DTO data)
        {
            return COMMM1.POSTDataExam(data, "ExamMarksprocessconditionFacade/saveUserPromotionData/");
        }
        //saveUserPromotionDataNew
        public ExamMarksProcess_DTO saveUserPromotionDataNew(ExamMarksProcess_DTO data)
        {
            return COMMM1.POSTDataExam(data, "ExamMarksprocessconditionFacade/saveUserPromotionDataNew/");
        }
        public ExamMarksProcess_DTO deActiveUserPromotion(ExamMarksProcess_DTO data)
        {
            return COMMM1.POSTDataExam(data, "ExamMarksprocessconditionFacade/deActiveUserPromotion/");
        }
        public ExamMarksProcess_DTO editUserPromotion(ExamMarksProcess_DTO data)
        {
            return COMMM1.POSTDataExam(data, "ExamMarksprocessconditionFacade/editUserPromotion/");
        }
           
    }
}

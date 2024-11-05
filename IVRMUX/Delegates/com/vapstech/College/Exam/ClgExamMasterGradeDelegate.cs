using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgExamMasterGradeDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterExamGradeDTO, MasterExamGradeDTO> COMMM = new CommonDelegate<MasterExamGradeDTO, MasterExamGradeDTO>();
        public MasterExamGradeDTO getdetails(int data)
        {
            return COMMM.GETexam(data, "ClgExamMasterGradeFacade/getdetails/");
        }
        public MasterExamGradeDTO savedetail(MasterExamGradeDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamMasterGradeFacade/savedetail/");
        }
        public MasterExamGradeDTO deactivate(MasterExamGradeDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamMasterGradeFacade/deactivate/");
        }
        public MasterExamGradeDTO getalldetailsviewrecords(int data)
        {
            return COMMM.GETexam(data, "ClgExamMasterGradeFacade/getalldetailsviewrecords/");
        }
        public MasterExamGradeDTO getpagedetails(int data)
        {
            return COMMM.GETexam(data, "ClgExamMasterGradeFacade/getpagedetails/");
        }
        public MasterExamGradeDTO deleterec(int data)
        {
            return COMMM.GETexam(data, "ClgExamMasterGradeFacade/deletedetails/");
        }
    }
}

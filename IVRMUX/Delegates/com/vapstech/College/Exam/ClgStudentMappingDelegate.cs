using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgStudentMappingDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Exm_Col_Studentwise_SubjectsDTO, Exm_Col_Studentwise_SubjectsDTO> COMMM = new CommonDelegate<Exm_Col_Studentwise_SubjectsDTO, Exm_Col_Studentwise_SubjectsDTO>();

        public Exm_Col_Studentwise_SubjectsDTO getdetails(int data)
        {
            return COMMM.GETexam(data, "ClgStudentMappingFacade/getdetails/");
        }
        public Exm_Col_Studentwise_SubjectsDTO Studentdetails(Exm_Col_Studentwise_SubjectsDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgStudentMappingFacade/Studentdetails/");
        }
        public Exm_Col_Studentwise_SubjectsDTO savedetails(Exm_Col_Studentwise_SubjectsDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgStudentMappingFacade/savedetails/");
        }
        public Exm_Col_Studentwise_SubjectsDTO getcourse(Exm_Col_Studentwise_SubjectsDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgStudentMappingFacade/getcourse/");
        }
        public Exm_Col_Studentwise_SubjectsDTO getbranch(Exm_Col_Studentwise_SubjectsDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgStudentMappingFacade/getbranch/");
        }
        public Exm_Col_Studentwise_SubjectsDTO getsemester(Exm_Col_Studentwise_SubjectsDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgStudentMappingFacade/getsemester/");
        }
        public Exm_Col_Studentwise_SubjectsDTO getsection(Exm_Col_Studentwise_SubjectsDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgStudentMappingFacade/getsection/");
        }
    }
}

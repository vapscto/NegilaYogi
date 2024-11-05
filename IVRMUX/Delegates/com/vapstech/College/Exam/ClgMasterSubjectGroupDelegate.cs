using CommonLibrary;
using PreadmissionDTOs.com.vaps.College;
using PreadmissionDTOs.com.vaps.College.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgMasterSubjectGroupDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterSubjectGroupDTO, MasterSubjectGroupDTO> COMMM = new CommonDelegate<MasterSubjectGroupDTO, MasterSubjectGroupDTO>();
        CommonDelegate<Exm_Col_Master_Group_SubjectsDTO, Exm_Col_Master_Group_SubjectsDTO> COMMM1 = new CommonDelegate<Exm_Col_Master_Group_SubjectsDTO, Exm_Col_Master_Group_SubjectsDTO>();
        public MasterSubjectGroupDTO getdetails(int data)
        {
            return COMMM.GETexam(data, "ClgMasterSubjectGroupFacade/getdetails/");
        }
        public MasterSubjectGroupDTO savedetail(MasterSubjectGroupDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgMasterSubjectGroupFacade/savedetail/");
        }
        public MasterSubjectGroupDTO deactivate(MasterSubjectGroupDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgMasterSubjectGroupFacade/deactivate/");
        }
        public Exm_Col_Master_Group_SubjectsDTO getalldetailsviewrecords(int data)
        {
            return COMMM1.GETexam(data, "ClgMasterSubjectGroupFacade/getalldetailsviewrecords/");
        }
        public MasterSubjectGroupDTO getpagedetails(int data)
        {
            return COMMM.GETexam(data, "ClgMasterSubjectGroupFacade/getpagedetails/");
        }
        public MasterSubjectGroupDTO deleterec(int data)
        {
            return COMMM.GETexam(data, "ClgMasterSubjectGroupFacade/deletedetails/");
        }
    }
}

using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgcoursebranchmappingDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Exm_Col_CourseBranchDTO, Exm_Col_CourseBranchDTO> COMMM = new CommonDelegate<Exm_Col_CourseBranchDTO, Exm_Col_CourseBranchDTO>();
        public Exm_Col_CourseBranchDTO editdeatils(int data)
        {
            return COMMM.GETexam(data, "ClgcoursebranchmappingFacade/editdeatils/");
        }
        public Exm_Col_CourseBranchDTO getdetails(int data)
        {
            return COMMM.GETexam(data, "ClgcoursebranchmappingFacade/getdetails/");
        }
        public Exm_Col_CourseBranchDTO getbranch(Exm_Col_CourseBranchDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgcoursebranchmappingFacade/getbranch/");
        }
        
        public Exm_Col_CourseBranchDTO savedetail2(Exm_Col_CourseBranchDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgcoursebranchmappingFacade/savedetail2/");
        }
        public Exm_Col_CourseBranchDTO get_subjects(Exm_Col_CourseBranchDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgcoursebranchmappingFacade/get_subjects/");
        }
        public Exm_Col_CourseBranchDTO getalldetailsviewrecords(Exm_Col_CourseBranchDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgcoursebranchmappingFacade/getalldetailsviewrecords/");
        }
        public Exm_Col_CourseBranchDTO deactivate(Exm_Col_CourseBranchDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgcoursebranchmappingFacade/deactivate/");
        }
    }
}

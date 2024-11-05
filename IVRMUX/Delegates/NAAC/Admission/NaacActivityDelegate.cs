using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NaacActivityDelegate
    {
        CommonDelegate<NaacActivity_DTO, NaacActivity_DTO> comm = new CommonDelegate<NaacActivity_DTO, NaacActivity_DTO>();

        public NaacActivity_DTO loaddata(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/loaddata");
        }
        public NaacActivity_DTO get_course(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/get_course");
        }
        public NaacActivity_DTO get_branch(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/get_branch");
        }
        public NaacActivity_DTO get_sems(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/get_sems");
        }
        public NaacActivity_DTO get_section(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/get_section");
        }
        public NaacActivity_DTO GetStudentDetails(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/GetStudentDetails");
        }
        public NaacActivity_DTO get_Designation(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/get_Designation");
        }
        public NaacActivity_DTO get_Employee(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/get_Employee");
        }
        public NaacActivity_DTO saverecord(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/saverecord");
        }
        public NaacActivity_DTO deactiveStudent(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/deactiveStudent");
        }
        public NaacActivity_DTO EditData(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/EditData");
        }
        public NaacActivity_DTO get_MappedStudent(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/get_MappedStudent");
        }
        public NaacActivity_DTO get_MappedStaff(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/get_MappedStaff");
        }
        public NaacActivity_DTO deactive_student(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/deactive_student");
        }
        public NaacActivity_DTO deactive_staff(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/deactive_staff");
        }
        public NaacActivity_DTO viewdocument_MainActUploadFiles(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/viewdocument_MainActUploadFiles");
        }
        public NaacActivity_DTO delete_MainActUploadFiles(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/delete_MainActUploadFiles");
        }
        public NaacActivity_DTO viewdocument_StudentActUploadFiles(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/viewdocument_StudentActUploadFiles");
        }
        public NaacActivity_DTO delete_StudentActUploadFiles(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/delete_StudentActUploadFiles");
        }
        public NaacActivity_DTO viewdocument_StaffActUploadFiles(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/viewdocument_StaffActUploadFiles");
        }
        public NaacActivity_DTO delete_StaffActUploadFiles(NaacActivity_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacActivityFacade/delete_StaffActUploadFiles");
        }

    }
}

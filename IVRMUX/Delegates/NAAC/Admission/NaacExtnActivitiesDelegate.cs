using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NaacExtnActivitiesDelegate
    {
        CommonDelegate<NAAC_AC_344_ExtnActivities_DTO, NAAC_AC_344_ExtnActivities_DTO> comm = new CommonDelegate<NAAC_AC_344_ExtnActivities_DTO, NAAC_AC_344_ExtnActivities_DTO>();

        public NAAC_AC_344_ExtnActivities_DTO loaddata(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/loaddata");
        }       
        public NAAC_AC_344_ExtnActivities_DTO get_branch(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/get_branch");
        }
        public NAAC_AC_344_ExtnActivities_DTO get_sems(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/get_sems");
        }
        public NAAC_AC_344_ExtnActivities_DTO get_section(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/get_section");
        }
        public NAAC_AC_344_ExtnActivities_DTO GetStudentDetails(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/GetStudentDetails");
        }
        public NAAC_AC_344_ExtnActivities_DTO saverecord(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/saverecord");
        }
        public NAAC_AC_344_ExtnActivities_DTO getcomment(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/getcomment");
        }
        public NAAC_AC_344_ExtnActivities_DTO getfilecomment(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/getfilecomment");
        }
        public NAAC_AC_344_ExtnActivities_DTO savemedicaldatawisecomments(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_344_ExtnActivities_DTO savefilewisecomments(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/savefilewisecomments");
        }
        public NAAC_AC_344_ExtnActivities_DTO deactiveStudent(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/deactiveStudent");
        }
        public NAAC_AC_344_ExtnActivities_DTO EditData(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/EditData");
        }
        public NAAC_AC_344_ExtnActivities_DTO get_MappedStudent(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/get_MappedStudent");
        }
        public NAAC_AC_344_ExtnActivities_DTO deactive_student(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/deactive_student");
        }
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_MainActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/viewdocument_MainActUploadFiles");
        }
        public NAAC_AC_344_ExtnActivities_DTO delete_MainActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/delete_MainActUploadFiles");
        }
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_StudentActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/viewdocument_StudentActUploadFiles");
        }
        public NAAC_AC_344_ExtnActivities_DTO delete_StudentActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/delete_StudentActUploadFiles");
        }
        public NAAC_AC_344_ExtnActivities_DTO get_Designation(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/get_Designation");
        }
        public NAAC_AC_344_ExtnActivities_DTO get_Employee(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/get_Employee");
        }
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_StaffActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/viewdocument_StaffActUploadFiles");
        }
        public NAAC_AC_344_ExtnActivities_DTO delete_StaffActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/delete_StaffActUploadFiles");
        }
        public NAAC_AC_344_ExtnActivities_DTO get_MappedStaff(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/get_MappedStaff");
        }
        public NAAC_AC_344_ExtnActivities_DTO deactive_staff(NAAC_AC_344_ExtnActivities_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacExtnActivitiesFacade/deactive_staff");
        }
    }
}

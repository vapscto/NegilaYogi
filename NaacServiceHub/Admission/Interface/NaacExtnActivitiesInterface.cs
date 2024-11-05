using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NaacExtnActivitiesInterface
    {
        NAAC_AC_344_ExtnActivities_DTO loaddata(NAAC_AC_344_ExtnActivities_DTO data);
        //NAAC_AC_344_ExtnActivities_DTO get_course(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO get_branch(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO get_sems(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO get_section(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO GetStudentDetails(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO saverecord(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO getcomment(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO getfilecomment(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO savemedicaldatawisecomments(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO savefilewisecomments(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO deactiveStudent(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO EditData(NAAC_AC_344_ExtnActivities_DTO data);
        Task<NAAC_AC_344_ExtnActivities_DTO> get_MappedStudent(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO deactive_student(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO viewdocument_MainActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO delete_MainActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO viewdocument_StudentActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO delete_StudentActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO get_Designation(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO get_Employee(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO viewdocument_StaffActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO delete_StaffActUploadFiles(NAAC_AC_344_ExtnActivities_DTO data);
        Task<NAAC_AC_344_ExtnActivities_DTO> get_MappedStaff(NAAC_AC_344_ExtnActivities_DTO data);
        NAAC_AC_344_ExtnActivities_DTO deactive_staff(NAAC_AC_344_ExtnActivities_DTO data);
    }
}

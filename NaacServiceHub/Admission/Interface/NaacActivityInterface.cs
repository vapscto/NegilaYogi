using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
    public interface NaacActivityInterface
    {
        NaacActivity_DTO loaddata(NaacActivity_DTO data);
        NaacActivity_DTO get_course(NaacActivity_DTO data);
        NaacActivity_DTO get_branch(NaacActivity_DTO data);
        NaacActivity_DTO get_sems(NaacActivity_DTO data);
        NaacActivity_DTO get_section(NaacActivity_DTO data);
        NaacActivity_DTO GetStudentDetails(NaacActivity_DTO data);
        NaacActivity_DTO get_Designation(NaacActivity_DTO data);
        NaacActivity_DTO get_Employee(NaacActivity_DTO data);
        NaacActivity_DTO saverecord(NaacActivity_DTO data);
        NaacActivity_DTO deactiveStudent(NaacActivity_DTO data);
        NaacActivity_DTO EditData(NaacActivity_DTO data);
        Task<NaacActivity_DTO> get_MappedStudent(NaacActivity_DTO data);
        Task<NaacActivity_DTO> get_MappedStaff(NaacActivity_DTO data);
        NaacActivity_DTO deactive_student(NaacActivity_DTO data);
        NaacActivity_DTO deactive_staff(NaacActivity_DTO data);
        NaacActivity_DTO viewdocument_MainActUploadFiles(NaacActivity_DTO data);
        NaacActivity_DTO delete_MainActUploadFiles(NaacActivity_DTO data);
        NaacActivity_DTO viewdocument_StudentActUploadFiles(NaacActivity_DTO data);
        NaacActivity_DTO delete_StudentActUploadFiles(NaacActivity_DTO data);
        NaacActivity_DTO viewdocument_StaffActUploadFiles(NaacActivity_DTO data);
        NaacActivity_DTO delete_StaffActUploadFiles(NaacActivity_DTO data);


    }
}

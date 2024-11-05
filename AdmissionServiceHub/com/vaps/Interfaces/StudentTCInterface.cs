using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudentTCInterface
    {
        Task<StudentTCDTO> GetStudentInitialData(StudentTCDTO MIID);
        StudentTCDTO gettcDetails(StudentTCDTO page);
        Task<StudentTCDTO> saveTcdet(StudentTCDTO pros);
        StudentTCDTO chk_tc_dup(StudentTCDTO dup_tc_no);
        Task<StudentTCDTO> getstudent_name_list(StudentTCDTO get_stu_name_list);
        StudentTCDTO getStatusDetails(StudentTCDTO status);
        StudentTCDTO searchfilter(StudentTCDTO data);

        // TC Cancel
        StudentTCDTO GetTCCancelDetails(StudentTCDTO data);
        StudentTCDTO OnChangeAcademicYear(StudentTCDTO data);
        StudentTCDTO OnStudentNameChange(StudentTCDTO data);
        StudentTCDTO SaveTCCancelDetails(StudentTCDTO data);
        //
        Task<StudentTCDTO> sourcecntdata(StudentTCDTO data);
        StudentTCDTO getallsourcedetails(StudentTCDTO data);
        Task<StudentTCDTO> languagecntdata(StudentTCDTO data);
        Task<StudentTCDTO> statecntdata(StudentTCDTO data);
        

    }
}
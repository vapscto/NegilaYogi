using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
   public interface StudentGatePassInterface
    {
        StudentGatePass_DTO getdetails(StudentGatePass_DTO dTO);
        StudentGatePass_DTO get_class(StudentGatePass_DTO mas);
        StudentGatePass_DTO get_section(StudentGatePass_DTO mas);
        StudentGatePass_DTO get_student(StudentGatePass_DTO mas);
        Task<StudentGatePass_DTO> saverecordAsync(StudentGatePass_DTO data);
        StudentGatePass_DTO editrecord(StudentGatePass_DTO id);
        StudentGatePass_DTO deactive(StudentGatePass_DTO data);
        StudentGatePass_DTO checkstudentdata(StudentGatePass_DTO data);
       StudentGatePass_DTO get_otpverification(StudentGatePass_DTO data);
        Task<StudentGatePass_DTO> resendotp(StudentGatePass_DTO data);
        StudentGatePass_DTO get_otpverification22(StudentGatePass_DTO data);
        StudentGatePass_DTO printbutton(StudentGatePass_DTO data);
        Task<StudentGatePass_DTO> GetStudDetails(StudentGatePass_DTO data);
        Task<StudentGatePass_DTO> getotp(StudentGatePass_DTO data);
    }
}

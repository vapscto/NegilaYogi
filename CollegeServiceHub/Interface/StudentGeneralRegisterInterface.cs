using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface StudentGeneralRegisterInterface
    {
        StudentGeneralRegisterDTO getdetails(StudentGeneralRegisterDTO data);
        StudentGeneralRegisterDTO onselectAcdYear(StudentGeneralRegisterDTO data);
        StudentGeneralRegisterDTO onselectCourse(StudentGeneralRegisterDTO data);
        StudentGeneralRegisterDTO onselectBranch(StudentGeneralRegisterDTO data);

        Task<StudentGeneralRegisterDTO> onreport(StudentGeneralRegisterDTO data);
    }
}

using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
   public interface CollegegeneralsmsInterface
    {
        Task<CollegegeneralsmsDTO> savedetail(CollegegeneralsmsDTO data);
        Task<CollegegeneralsmsDTO> GetStudentDetails(CollegegeneralsmsDTO data);
        Task<CollegegeneralsmsDTO> Getdetails(CollegegeneralsmsDTO data);
        CollegegeneralsmsDTO GetEmployeeDetailsByLeaveYearAndMonth(CollegegeneralsmsDTO data);
        CollegegeneralsmsDTO Getdepartment(CollegegeneralsmsDTO data);
        CollegegeneralsmsDTO get_designation(CollegegeneralsmsDTO data);
        CollegegeneralsmsDTO get_employee(CollegegeneralsmsDTO data);
        CollegegeneralsmsDTO Getexam(CollegegeneralsmsDTO data);
        CollegegeneralsmsDTO onSelectyear(CollegegeneralsmsDTO data);
        CollegegeneralsmsDTO onselectedcourse(CollegegeneralsmsDTO data);
        CollegegeneralsmsDTO onselectbranch(CollegegeneralsmsDTO data);
        CollegegeneralsmsDTO onselectsemister(CollegegeneralsmsDTO data);
    }
}

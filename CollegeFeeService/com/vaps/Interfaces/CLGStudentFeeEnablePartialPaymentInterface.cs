using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Fee;

namespace CollegeFeeService.com.vaps.Interfaces
{
   public interface CLGStudentFeeEnablePartialPaymentInterface
    {
        CollegeOverallFeeStatusDTO GetYearList(int id);
        CollegeOverallFeeStatusDTO get_courses(CollegeOverallFeeStatusDTO id);
        CollegeOverallFeeStatusDTO get_branches(CollegeOverallFeeStatusDTO data);
        CollegeOverallFeeStatusDTO get_semisters(CollegeOverallFeeStatusDTO data);
        CollegeOverallFeeStatusDTO get_student(CollegeOverallFeeStatusDTO data);
        CollegeOverallFeeStatusDTO savedata(CollegeOverallFeeStatusDTO data);
        CollegeOverallFeeStatusDTO deactivate(CollegeOverallFeeStatusDTO data);

    }
}

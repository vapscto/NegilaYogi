using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
   public interface CollegeOverallFeeStatusInterface
    {
        CollegeOverallFeeStatusDTO GetYearList(int id);
        CollegeOverallFeeStatusDTO get_courses(CollegeOverallFeeStatusDTO id);
        CollegeOverallFeeStatusDTO get_branches(CollegeOverallFeeStatusDTO data);
        CollegeOverallFeeStatusDTO get_semisters(CollegeOverallFeeStatusDTO data);

         CollegeOverallFeeStatusDTO get_report(CollegeOverallFeeStatusDTO data);
        CollegeOverallFeeStatusDTO savedata(CollegeOverallFeeStatusDTO data);
        CollegeOverallFeeStatusDTO editdata(CollegeOverallFeeStatusDTO data);
        CollegeOverallFeeStatusDTO DeleteRecord(CollegeOverallFeeStatusDTO data);

    }
}

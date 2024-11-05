using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
  public interface FeeGroupClgInterface
    {
        FeeGroupClgDTO SaveGroupData(FeeGroupClgDTO org);
        FeeGroupClgDTO EditgroupDetails(int id);
        FeeGroupClgDTO getdetails(FeeGroupClgDTO data);
        FeeGroupClgDTO GetGroupSearchData(FeeGroupClgDTO mas);
        FeeGroupClgDTO getpageedit(int id);
        FeeGroupClgDTO deleterec(int id);
        FeeGroupClgDTO deactivate(FeeGroupClgDTO id);

        //for yearly 

        Task<FeeGroupClgDTO> getIndependentDropDowns(FeeGroupClgDTO yrs);


        //  Task<FeeYearlyGroupClgDTO> getIndependentDrop(FeeYearlyGroupClgDTO yrs);
        FeeYearlyGroupClgDTO SaveYearlyGroupData(int id, FeeYearlyGroupClgDTO org);

        FeeYearlyGroupClgDTO getdetailsY(int id);
        FeeYearlyGroupClgDTO deactivateY(FeeYearlyGroupClgDTO id);
        FeeYearlyGroupClgDTO getpageeditY(int id);


        FeeYearlyGroupClgDTO deleterecY(int id);

        FeeYearlyGroupClgDTO selectacade(FeeYearlyGroupClgDTO data);
    }
}

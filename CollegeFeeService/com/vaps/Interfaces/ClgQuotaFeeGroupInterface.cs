using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface ClgQuotaFeeGroupInterface
    {
        ClgQuotaFeeGroupDTO SaveGroupData(int id, ClgQuotaFeeGroupDTO org);
        ClgQuotaFeeGroupDTO EditgroupDetails(int id);
        ClgQuotaFeeGroupDTO getdetails(ClgQuotaFeeGroupDTO data);
        ClgQuotaFeeGroupDTO GetGroupSearchData(ClgQuotaFeeGroupDTO mas);
        ClgQuotaFeeGroupDTO getpageedit(int id);
      
        ClgQuotaFeeGroupDTO deactivate(ClgQuotaFeeGroupDTO id);
        //Task<ClgQuotaFeeGroupDTO> getIndependentDropDowns(ClgQuotaFeeGroupDTO yrs);




    }
}

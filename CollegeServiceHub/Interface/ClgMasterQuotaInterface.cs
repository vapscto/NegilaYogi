using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface ClgMasterQuotaInterface
    {
        ClgQuotaDTO getalldetails(ClgQuotaDTO data);
        //---------------------Quota MAster
        ClgQuotaDTO savedetails(ClgQuotaDTO data);
        ClgQuotaDTO editdetails(ClgQuotaDTO data);
        ClgQuotaDTO activedeactiveQuota(ClgQuotaDTO data);
        //---------------------Quota Category
        ClgQuotaDTO savedetails1(ClgQuotaDTO data);
        ClgQuotaDTO editdetails1(ClgQuotaDTO data);
        ClgQuotaDTO activedeactiveQuota1(ClgQuotaDTO data);

        //---------------------Quota Category Mapping
        ClgQuotaDTO savedetails2(ClgQuotaDTO data);
        ClgQuotaDTO activedeactiveQuota2(ClgQuotaDTO data);

    }
    
}

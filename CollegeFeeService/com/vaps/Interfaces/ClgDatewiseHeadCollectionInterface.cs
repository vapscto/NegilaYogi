using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
   public interface ClgDatewiseHeadCollectionInterface
    {
        ClgDatewiseHeadCollectionDTO GetYearList(int id);
        ClgDatewiseHeadCollectionDTO get_feegroups(ClgDatewiseHeadCollectionDTO id);
        ClgDatewiseHeadCollectionDTO get_heads(ClgDatewiseHeadCollectionDTO data);
        ClgDatewiseHeadCollectionDTO get_semisters(ClgDatewiseHeadCollectionDTO data);

         ClgDatewiseHeadCollectionDTO get_report(ClgDatewiseHeadCollectionDTO data);
        ClgDatewiseHeadCollectionDTO savedata(ClgDatewiseHeadCollectionDTO data);
        ClgDatewiseHeadCollectionDTO editdata(ClgDatewiseHeadCollectionDTO data);
        ClgDatewiseHeadCollectionDTO DeleteRecord(ClgDatewiseHeadCollectionDTO data);

    }
}

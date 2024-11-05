using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
  public  interface FeeHeadClgInterface
    {
        FeeHeadClgDTO SaveGroupData(FeeHeadClgDTO org);
        FeeHeadClgDTO changeorderData(FeeHeadClgDTO org);
        FeeHeadClgDTO EditgroupDetails(int id);
        FeeHeadClgDTO getdetails(int id);
        FeeHeadClgDTO GetGroupSearchData(FeeHeadClgDTO mas);
        FeeHeadClgDTO getpageedit(int id);
        FeeHeadClgDTO deleterec(int id);
        FeeHeadClgDTO deactivate(FeeHeadClgDTO id);


        FeeHeadClgDTO getallbankdetails(FeeHeadClgDTO data);
        FeeHeadClgDTO savedata(FeeHeadClgDTO data);
        FeeHeadClgDTO edit(FeeHeadClgDTO data);
        FeeHeadClgDTO activedeactive(FeeHeadClgDTO data);

    }
}

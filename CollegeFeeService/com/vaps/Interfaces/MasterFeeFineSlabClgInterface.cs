using PreadmissionDTOs.com.vaps.College.Fee;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
  public  interface MasterFeeFineSlabClgInterface
    {
        MasterFeeFineSlabClg_DTO SaveGroupData(MasterFeeFineSlabClg_DTO org);
        MasterFeeFineSlabClg_DTO EditgroupDetails(int id);
        MasterFeeFineSlabClg_DTO getdetails(int id);
        MasterFeeFineSlabClg_DTO GetGroupSearchData(MasterFeeFineSlabClg_DTO mas);
        MasterFeeFineSlabClg_DTO getpageedit(int id);
        MasterFeeFineSlabClg_DTO deleterec(int id);
        MasterFeeFineSlabClg_DTO deactivate(MasterFeeFineSlabClg_DTO id);
    }
}

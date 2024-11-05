using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;


namespace FeeServiceHub.com.vaps.interfaces
{
   public interface FeeFineSlabInterface
    {
        FeeFineSlabDTO SaveGroupData(FeeFineSlabDTO org);
        FeeFineSlabDTO EditgroupDetails(int id);
        FeeFineSlabDTO getdetails(int id);
        FeeFineSlabDTO GetGroupSearchData(FeeFineSlabDTO mas);
        FeeFineSlabDTO getpageedit(int id);
        FeeFineSlabDTO deleterec(int id);
        FeeFineSlabDTO deactivate(FeeFineSlabDTO id);
    }
}

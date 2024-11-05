using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeePDCInterface
    {
        FeePDCDTO SaveGroupData(FeePDCDTO org);

        FeePDCDTO EditgroupDetails(int id);
        FeePDCDTO getdetails(int id);
        FeePDCDTO GetGroupSearchData(FeePDCDTO mas);
        FeePDCDTO getpageedit(int id);
        FeePDCDTO deactivate(FeePDCDTO id);
        FeePDCDTO PDCRemainder(FeePDCDTO data);

    }
}

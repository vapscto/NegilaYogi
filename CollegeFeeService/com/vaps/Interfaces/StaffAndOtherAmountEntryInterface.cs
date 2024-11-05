
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface StaffAndOtherAmountEntryInterface
    {
        FeeAmountEntryDTO getdata(FeeAmountEntryDTO data);
        FeeAmountEntryDTO savedetails(FeeAmountEntryDTO pgmod);
        FeeAmountEntryDTO deleterec(FeeAmountEntryDTO data);
        FeeAmountEntryDTO getsearchdata(int id, FeeAmountEntryDTO org);
        FeeAmountEntryDTO EditMasterscetionDetails(FeeAmountEntryDTO data);

        FeeAmountEntryDTO paymentdetailsfnc(FeeAmountEntryDTO id);

        FeeAmountEntryDTO getgroupheaddetails(FeeAmountEntryDTO data);

        FeeAmountEntryDTO selectacade(FeeAmountEntryDTO data);
        FeeAmountEntryDTO getalldetailsOnselectiontype(FeeAmountEntryDTO data);
    }
}

using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeAmountEntryStthomasInterface
    {
        FeeAmountEntryStthomasDTO getdata(FeeAmountEntryStthomasDTO data);
        FeeAmountEntryStthomasDTO savedetails(FeeAmountEntryStthomasDTO pgmod);
        FeeAmountEntryStthomasDTO deleterec(FeeAmountEntryStthomasDTO data);
        FeeAmountEntryStthomasDTO getsearchdata(int id, FeeAmountEntryStthomasDTO org);
        FeeAmountEntryStthomasDTO EditMasterscetionDetails(FeeAmountEntryStthomasDTO data);

        FeeAmountEntryStthomasDTO paymentdetailsfnc(FeeAmountEntryStthomasDTO id);

        FeeAmountEntryStthomasDTO getgroupheaddetails(FeeAmountEntryStthomasDTO data);

        FeeAmountEntryStthomasDTO selectacade(FeeAmountEntryStthomasDTO data);
        FeeAmountEntryStthomasDTO getalldetailsOnselectiontype(FeeAmountEntryStthomasDTO data);

    }
}

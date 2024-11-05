using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CLGFeeChequeBounceInterface
    {
        CLGFeeChequeBounceDTO getalldetails(CLGFeeChequeBounceDTO data);
        CLGFeeChequeBounceDTO get_students(CLGFeeChequeBounceDTO data);
        CLGFeeChequeBounceDTO get_receipts(CLGFeeChequeBounceDTO data);
        CLGFeeChequeBounceDTO savedata(CLGFeeChequeBounceDTO data);
        CLGFeeChequeBounceDTO DeletRecord(CLGFeeChequeBounceDTO data);
    }
}

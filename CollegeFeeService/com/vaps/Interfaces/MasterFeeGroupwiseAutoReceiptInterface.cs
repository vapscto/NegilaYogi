using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface MasterFeeGroupwiseAutoReceiptInterface
    {
        MasterFeeGroupwiseAutoReceiptDTO getinitialdata(MasterFeeGroupwiseAutoReceiptDTO data);
        MasterFeeGroupwiseAutoReceiptDTO svedata(MasterFeeGroupwiseAutoReceiptDTO data);
        MasterFeeGroupwiseAutoReceiptDTO editdta(MasterFeeGroupwiseAutoReceiptDTO data);
        MasterFeeGroupwiseAutoReceiptDTO deletedta(MasterFeeGroupwiseAutoReceiptDTO data);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeCollectionReportInterface
    {
        FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO data);

        Task<FeeTransactionPaymentDTO> getradiofiltereddata(FeeTransactionPaymentDTO temp);
        FeeTransactionPaymentDTO get_groups(FeeTransactionPaymentDTO data);
        FeeTransactionPaymentDTO getgrpterms(FeeTransactionPaymentDTO data);
        
    }
}

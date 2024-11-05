using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
 public  interface FeeClasswiseConcessionReportInterface
    {
        FeeTransactionPaymentDTO getdetails(int id);

        Task<FeeTransactionPaymentDTO> concessiondtl(FeeTransactionPaymentDTO temp);
    }
}

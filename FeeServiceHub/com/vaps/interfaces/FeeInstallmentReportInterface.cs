using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeInstallmentReportInterface
    {
        FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO dt);

        Task<FeeTransactionPaymentDTO> radiobtndata(FeeTransactionPaymentDTO data);

        FeeTransactionPaymentDTO getinstallmentid(FeeTransactionPaymentDTO data);

        FeeTransactionPaymentDTO getinstallmentid1(FeeTransactionPaymentDTO data);

        FeeTransactionPaymentDTO classes(FeeTransactionPaymentDTO data);
        FeeTransactionPaymentDTO group(FeeTransactionPaymentDTO data);

    }
}

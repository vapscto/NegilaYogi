using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeDetailsReportInterface
    {
        FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO data);

        FeeTransactionPaymentDTO getheadisbygrpid(FeeTransactionPaymentDTO data);

        FeeTransactionPaymentDTO getsection(FeeTransactionPaymentDTO data);

        Task<FeeTransactionPaymentDTO> radiobtndata(FeeTransactionPaymentDTO temp);



    }
}

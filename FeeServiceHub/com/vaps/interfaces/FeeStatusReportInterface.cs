using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
   public interface FeeStatusReportInterface
    {
        FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO dto);

        Task<FeeTransactionPaymentDTO> radiobtndata(FeeTransactionPaymentDTO temp);
        FeeTransactionPaymentDTO get_fee_and_stu(FeeTransactionPaymentDTO temp);
        FeeTransactionPaymentDTO getfeehead_statement_report(FeeTransactionPaymentDTO temp);
        FeeTransactionPaymentDTO getsection(FeeTransactionPaymentDTO temp);
        FeeTransactionPaymentDTO GetSettlementReport(FeeTransactionPaymentDTO temp);
    }
}

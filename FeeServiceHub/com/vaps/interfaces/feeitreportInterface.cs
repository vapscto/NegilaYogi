using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface feeitreportInterface
    {
        FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO dt);

        FeeTransactionPaymentDTO getsection(FeeTransactionPaymentDTO data);

        FeeTransactionPaymentDTO getstudent(FeeTransactionPaymentDTO data);
        FeeTransactionPaymentDTO getreceipt(FeeTransactionPaymentDTO data);

        Task<FeeStudentTransactionDTO> printreceipt(FeeStudentTransactionDTO data);
        FeeTransactionPaymentDTO getreceiptreport(FeeTransactionPaymentDTO data);

    }
}

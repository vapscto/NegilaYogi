using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface PreadmissionOnlinePaymentInterface
    {
        FeeStudentTransactionDTO getdetails(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO getstudentdetails(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO hashgeneration(FeeStudentTransactionDTO data);
        PaymentDetails payuresponse(PaymentDetails data);

        FeeStudentTransactionDTO getamountdetails(FeeStudentTransactionDTO data);

        PaymentDetails.PAYTM payuresponsepaytm(PaymentDetails.PAYTM data);
        PaymentDetails razorgetpaymentresponse(PaymentDetails data);
        FeeStudentTransactionDTO Razorpaypaymentsettlementresponse(FeeStudentTransactionDTO data);
        PaymentDetails paytmresponsse(PaymentDetails data);
        PaymentDetails.easybuzz getpaymentresponseeasybuzz(PaymentDetails.easybuzz response);
    }
}

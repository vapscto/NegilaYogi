using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CollegePreAdmOnlinePaymentInterface
    {
        CollegeFeeTransactionDTO getdetails(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO getstudentdetails(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO hashgeneration(CollegeFeeTransactionDTO data);
        PaymentDetails payuresponse(PaymentDetails data);

        CollegeFeeTransactionDTO getamountdetails(CollegeFeeTransactionDTO data);

        PaymentDetails.PAYTM payuresponsepaytm(PaymentDetails.PAYTM data);
        PaymentDetails razorgetpaymentresponse(PaymentDetails data);
        CollegeFeeTransactionDTO Razorpaypaymentsettlementresponse(CollegeFeeTransactionDTO data);
        PaymentDetails paytmresponsse(PaymentDetails data);
    }
}

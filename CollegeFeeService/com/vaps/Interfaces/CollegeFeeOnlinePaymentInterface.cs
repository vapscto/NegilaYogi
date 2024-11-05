using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
   public interface CollegeFeeOnlinePaymentInterface
    {

        CollegeFeeTransactionDTO pageload(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO getheaddetails(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO generatehashsequence(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO generatehashsequencedisplay(CollegeFeeTransactionDTO data);

        PaymentDetails payuresponse(PaymentDetails data);

        PaymentDetails.PAYTM payuresponsepaytm(PaymentDetails.PAYTM data);

        PaymentDetails getpaymentresponserazorpay(PaymentDetails data);
        PaymentDetails.easybuzz PaymentEasebuzzResponse(PaymentDetails.easybuzz data);

        Task<CollegeFeeTransactionDTO> mobilepayuconnect(CollegeFeeTransactionDTO dto);
        PaymentDetails.easybuzzmobile getpaymentresponseeasybuzzmobile(PaymentDetails.easybuzzmobile response);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs;

namespace FeeServiceHub.com.vaps.interfaces
{
 public   interface EBSonlinepaymentInterface
    {
        FeeStudentTransactionDTO getdetails(FeeStudentTransactionDTO org);
        PaymentDetails payuresponse(PaymentDetails response);
    }
}

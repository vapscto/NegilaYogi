using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
   public interface FeePaymentGatewayDetailsInterface
    {
        Fee_PaymentGateway_DetailsDTO getPaymentGatewayDetails(int mi_id);
        Fee_PaymentGateway_DetailsDTO savePaymentGatewayDetails(Fee_PaymentGateway_DetailsDTO data);
        Fee_PaymentGateway_DetailsDTO editPaymentGatewayDetails(int id);
        Fee_PaymentGateway_DetailsDTO deletePaymentGatewayDetails(int id);
    }
}

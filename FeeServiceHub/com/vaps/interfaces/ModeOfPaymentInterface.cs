using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
   public interface ModeOfPaymentInterface
    {
        ModeOfPaymentDTO loaddata(ModeOfPaymentDTO data);
        ModeOfPaymentDTO savedata(ModeOfPaymentDTO data);
        ModeOfPaymentDTO paymentDecative(ModeOfPaymentDTO data);
        ModeOfPaymentDTO deletedata(ModeOfPaymentDTO data);
    }
}

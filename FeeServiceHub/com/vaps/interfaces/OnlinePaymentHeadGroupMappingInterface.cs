using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
   public interface OnlinePaymentHeadGroupMappingInterface
    {
        Fee_OnlinePayment_MappingDTO getDetails(int mi_id);
        Fee_OnlinePayment_MappingDTO saveDetails(Fee_OnlinePayment_MappingDTO data);
        Fee_OnlinePayment_MappingDTO editDetails(int id);
        Fee_OnlinePayment_MappingDTO deleteDetails(int id);

        Fee_OnlinePayment_MappingDTO selecgroup(Fee_OnlinePayment_MappingDTO data);

        Fee_OnlinePayment_MappingDTO selechead(Fee_OnlinePayment_MappingDTO data);
        Fee_OnlinePayment_MappingDTO acde(Fee_OnlinePayment_MappingDTO data);
    }
}

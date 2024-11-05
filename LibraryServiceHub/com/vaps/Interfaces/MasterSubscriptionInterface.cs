using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterSubscriptionInterface
    {

        Task<Master_Subscription_DTO> getdetails(Master_Subscription_DTO data);
        Master_Subscription_DTO Savedata(Master_Subscription_DTO data);
        Master_Subscription_DTO EditData(Master_Subscription_DTO data);       
       Master_Subscription_DTO deactiveY(Master_Subscription_DTO data);
      

    }
}

using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class MasterSubscriptionDelegate
    {

        CommonDelegate<Master_Subscription_DTO, Master_Subscription_DTO> _commnbranch = new CommonDelegate<Master_Subscription_DTO, Master_Subscription_DTO>();

        public Master_Subscription_DTO getdetails(Master_Subscription_DTO data)
        {
            return _commnbranch.PostLibrary(data, "MasterSubscriptionFacade/getdetails/");
        }

        public Master_Subscription_DTO Savedata(Master_Subscription_DTO data)
        {
            return _commnbranch.PostLibrary(data, "MasterSubscriptionFacade/Savedata/");
        }
        public Master_Subscription_DTO EditData(Master_Subscription_DTO data)
        {
            return _commnbranch.PostLibrary(data, "MasterSubscriptionFacade/EditData/");
        }

        public Master_Subscription_DTO deactiveY(Master_Subscription_DTO data)
        {
            return _commnbranch.PostLibrary(data, "MasterSubscriptionFacade/deactiveY/");
        }
        

       

    }
}

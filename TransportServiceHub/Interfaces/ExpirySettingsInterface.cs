using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public  interface ExpirySettingsInterface
    {
        ExpirySettingsDTO getdata(int id);
        ExpirySettingsDTO savedata(ExpirySettingsDTO data);
        ExpirySettingsDTO getdatadetails(ExpirySettingsDTO data);
        ExpirySettingsDTO getsmsdetails(ExpirySettingsDTO data);
        ExpirySettingsDTO jshsgetsmsdetails(ExpirySettingsDTO data);

        
    }
}

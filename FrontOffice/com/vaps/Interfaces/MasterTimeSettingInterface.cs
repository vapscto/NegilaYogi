using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
   public interface MasterTimeSettingInterface
    {
        MasterTimeSettingDTO savedetail(MasterTimeSettingDTO objcategory);
        MasterTimeSettingDTO getdetails(int id);
        MasterTimeSettingDTO getpageedit(int id);
        MasterTimeSettingDTO deleterec(int id);
    }
}

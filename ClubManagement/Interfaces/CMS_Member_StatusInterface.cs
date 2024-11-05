using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Interfaces
{
   public interface CMS_Member_StatusInterface
    {
        CMS_Member_StatusDTO loaddata(int dto);
        CMS_Member_StatusDTO savedata(CMS_Member_StatusDTO data);
        //deactive
        CMS_Member_StatusDTO deactive(CMS_Member_StatusDTO data);
    }
}

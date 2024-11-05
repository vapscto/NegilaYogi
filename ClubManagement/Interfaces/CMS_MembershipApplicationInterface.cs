using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Interfaces
{
   public interface CMS_MembershipApplicationInterface
    {
        CMS_MembershipApplicationDTO loaddata(int dto);
        CMS_MembershipApplicationDTO savedata(CMS_MembershipApplicationDTO data);
        //deactive
        CMS_MembershipApplicationDTO deactive(CMS_MembershipApplicationDTO data);
    }
}

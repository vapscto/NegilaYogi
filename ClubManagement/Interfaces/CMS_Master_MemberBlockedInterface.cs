using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Interfaces
{
   public  interface CMS_Master_MemberBlockedInterface
    {
        CMS_Master_MemberBlockedDTO savedetail1(CMS_Master_MemberBlockedDTO data);
        //deactive
      
        CMS_Master_MemberBlockedDTO loaddata(int id);
        CMS_Master_MemberBlockedDTO deactive(CMS_Master_MemberBlockedDTO data);
        //
    }
}

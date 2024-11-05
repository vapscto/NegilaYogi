using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Interfaces
{
  public  interface CMS_MemberCategoryInterface
    {
        CMS_MemberCategoryDTO loaddata(int dto);
        CMS_MemberCategoryDTO savedata(CMS_MemberCategoryDTO data);
        //deactive
        CMS_MemberCategoryDTO deactive(CMS_MemberCategoryDTO data);
        //edit
        CMS_MemberCategoryDTO edit(CMS_MemberCategoryDTO data);

    }
}

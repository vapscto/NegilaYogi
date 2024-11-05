using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Interfaces
{
   public interface CMS_MasterMemberInerface
    {
      //CMS_MastermemberDTO loaddata(int dto);
        CMS_MastermemberDTO savedetail1(CMS_MastermemberDTO data);
        //deactive
        CMS_MastermemberDTO deactive(CMS_MastermemberDTO data);
        CMS_MastermemberDTO loaddata(int id);
        CMS_MastermemberDTO editmember(CMS_MastermemberDTO data);
        CMS_Master_Member_QualificationDTO savedetail2(CMS_Master_Member_QualificationDTO data);
        //
        CMS_Master_Member_ExperienceDTO savedetail3(CMS_Master_Member_ExperienceDTO data);
        //editmember
        CMS_Master_MemberMobileNoDTO savedetail5(CMS_Master_MemberMobileNoDTO data);
        CMS_Master_Member_EmailDTO savedetail6(CMS_Master_Member_EmailDTO data);
        //savedetail7
        CMS_MasterMember_DocumentsDTO savedetail7(CMS_MasterMember_DocumentsDTO data);
    }
}

using CommonLibrary;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.ClubManagement
{
    public class CMS_Master_MemberBlockedDelegate
    {
        CommonDelegate<CMS_Master_MemberBlockedDTO, CMS_Master_MemberBlockedDTO> COMMM = new CommonDelegate<CMS_Master_MemberBlockedDTO, CMS_Master_MemberBlockedDTO>();
        public CMS_Master_MemberBlockedDTO loaddata(int id)
        {
            return COMMM.GetDataByClubManagement(id, "CMS_Master_MemberBlockedFacade/loaddata/");
        }
        //POSTDataClubManagement
        public CMS_Master_MemberBlockedDTO savedetail1(CMS_Master_MemberBlockedDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_Master_MemberBlockedFacade/savedetail1/");
        }
        //deactive
        public CMS_Master_MemberBlockedDTO deactive(CMS_Master_MemberBlockedDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_Master_MemberBlockedFacade/deactive/");
        }

    }
}

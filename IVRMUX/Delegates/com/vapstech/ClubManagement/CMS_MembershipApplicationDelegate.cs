using CommonLibrary;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.ClubManagement
{
    public class CMS_MembershipApplicationDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CMS_MembershipApplicationDTO, CMS_MembershipApplicationDTO> COMMM = new CommonDelegate<CMS_MembershipApplicationDTO, CMS_MembershipApplicationDTO>();
        public CMS_MembershipApplicationDTO loaddata(int id)
        {
            return COMMM.GetDataByClubManagement(id, "CMS_MembershipApplicationFacade/loaddata/");
        }
        //POSTDataClubManagement
        public CMS_MembershipApplicationDTO savedata(CMS_MembershipApplicationDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_MembershipApplicationFacade/savedata/");
        }
        //deactive
        public CMS_MembershipApplicationDTO deactive(CMS_MembershipApplicationDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_MembershipApplicationFacade/deactive/");
        }

    }
}

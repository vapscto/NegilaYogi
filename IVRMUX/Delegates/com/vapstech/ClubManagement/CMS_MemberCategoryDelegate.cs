using CommonLibrary;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.ClubManagement
{
    public class CMS_MemberCategoryDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CMS_MemberCategoryDTO, CMS_MemberCategoryDTO> COMMM = new CommonDelegate<CMS_MemberCategoryDTO, CMS_MemberCategoryDTO>();
        public CMS_MemberCategoryDTO loaddata(int id)
        {
            return COMMM.GetDataByClubManagement(id, "CMS_MemberCategoryFacade/loaddata/");
        }
        //POSTDataClubManagement
        public CMS_MemberCategoryDTO savedata(CMS_MemberCategoryDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_MemberCategoryFacade/savedata/");
        }
        //deactive
        public CMS_MemberCategoryDTO deactive(CMS_MemberCategoryDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_MemberCategoryFacade/deactive/");
        }
        //edit
        public CMS_MemberCategoryDTO edit(CMS_MemberCategoryDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_MemberCategoryFacade/edit/");
        }

    }
}

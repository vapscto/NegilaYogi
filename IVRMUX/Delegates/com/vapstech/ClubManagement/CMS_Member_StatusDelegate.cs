using CommonLibrary;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.ClubManagement
{
    public class CMS_Member_StatusDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CMS_Member_StatusDTO, CMS_Member_StatusDTO> COMMM = new CommonDelegate<CMS_Member_StatusDTO, CMS_Member_StatusDTO>();
        public CMS_Member_StatusDTO loaddata(int id)
        {
            return COMMM.GetDataByClubManagement(id, "CMS_Member_StatusFacade/loaddata/");
        }

        //POSTDataClubManagement
        public CMS_Member_StatusDTO savedata(CMS_Member_StatusDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_Member_StatusFacade/savedata/");
        }
        //deactive
        public CMS_Member_StatusDTO deactive(CMS_Member_StatusDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_Member_StatusFacade/deactive/");
        }

    }
}

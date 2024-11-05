using CommonLibrary;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.ClubManagement
{
    public class CMS_Master_InstallmentsDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CMS_Master_InstallmentsDTO, CMS_Master_InstallmentsDTO> COMMM = new CommonDelegate<CMS_Master_InstallmentsDTO, CMS_Master_InstallmentsDTO>();
        public CMS_Master_InstallmentsDTO loaddata(int id)
        {
            return COMMM.GetDataByClubManagement(id, "CMS_Master_InstallmentsFacade/loaddata/");
        }
        //POSTDataClubManagement
        public CMS_Master_InstallmentsDTO savedata(CMS_Master_InstallmentsDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_Master_InstallmentsFacade/savedata/");
        }
        //deactive
        public CMS_Master_InstallmentsDTO deactive(CMS_Master_InstallmentsDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_Master_InstallmentsFacade/deactive/");
        }
    }
}

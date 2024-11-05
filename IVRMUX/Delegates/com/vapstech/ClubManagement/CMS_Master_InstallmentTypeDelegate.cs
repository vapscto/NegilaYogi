using CommonLibrary;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.ClubManagement
{
    public class CMS_Master_InstallmentTypeDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CMS_Master_InstallmentTypeDTO, CMS_Master_InstallmentTypeDTO> COMMM = new CommonDelegate<CMS_Master_InstallmentTypeDTO, CMS_Master_InstallmentTypeDTO>();
        public CMS_Master_InstallmentTypeDTO loaddata(int id)
        {
            return COMMM.GetDataByClubManagement(id, "CMS_Master_InstallmentTypeFacade/loaddata/");
        }
        //POSTDataClubManagement
        public CMS_Master_InstallmentTypeDTO savedata(CMS_Master_InstallmentTypeDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_Master_InstallmentTypeFacade/savedata/");
        }
        //deactive
        public CMS_Master_InstallmentTypeDTO deactive(CMS_Master_InstallmentTypeDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_Master_InstallmentTypeFacade/deactive/");
        }
    }
}

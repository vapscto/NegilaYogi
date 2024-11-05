using CommonLibrary;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.ClubManagement
{
    public class CMS_MasterDepartmentDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CMS_MasterDepartmentDTO, CMS_MasterDepartmentDTO> COMMM = new CommonDelegate<CMS_MasterDepartmentDTO, CMS_MasterDepartmentDTO>();
        //CMS_ConfigurationDTO
        CommonDelegate<CMS_ConfigurationDTO, CMS_ConfigurationDTO> COMMC = new CommonDelegate<CMS_ConfigurationDTO, CMS_ConfigurationDTO>();
        public CMS_MasterDepartmentDTO loaddata(int id)
        {
            return COMMM.GetDataByClubManagement(id, "CMS_MasterDepartmentFacade/loaddata/");
        }

        //POSTDataClubManagement
        public CMS_MasterDepartmentDTO savedata(CMS_MasterDepartmentDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_MasterDepartmentFacade/savedata/");
        }
        //deactive
        public CMS_MasterDepartmentDTO deactive(CMS_MasterDepartmentDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_MasterDepartmentFacade/deactive/");
        }
        //loaddataconfigure
        public CMS_ConfigurationDTO loaddataconfigure(int id)
        {
            return COMMC.GetDataByClubManagement(id, "CMS_MasterDepartmentFacade/loaddataconfigure/");
        }
        public CMS_ConfigurationDTO confdeactive(CMS_ConfigurationDTO data)
        {
            return COMMC.POSTDataClubManagement(data, "CMS_MasterDepartmentFacade/confdeactive/");
        }
        public CMS_ConfigurationDTO saveconfigure(CMS_ConfigurationDTO data)
        {
            return COMMC.POSTDataClubManagement(data, "CMS_MasterDepartmentFacade/saveconfigure/");
        }
    }
}

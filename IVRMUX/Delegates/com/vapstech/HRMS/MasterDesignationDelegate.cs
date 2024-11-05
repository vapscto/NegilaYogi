using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterDesignationDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_DesignationDTO, HR_Master_DesignationDTO> COMMM = new CommonDelegate<HR_Master_DesignationDTO, HR_Master_DesignationDTO>();

        public HR_Master_DesignationDTO onloadgetdetails(HR_Master_DesignationDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterDesignationFacade/onloadgetdetails");
        }

        public HR_Master_DesignationDTO Onchangedetails(HR_Master_DesignationDTO dto)
        {
                  return COMMM.POSTDataHRMS(dto, "MasterDesignationFacade/Onchangedetails");
        }

    public HR_Master_DesignationDTO savedetails(HR_Master_DesignationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterDesignationFacade/");
        }
        public HR_Master_DesignationDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterDesignationFacade/getRecordById/");
        }
        public HR_Master_DesignationDTO deleterec(HR_Master_DesignationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterDesignationFacade/deactivateRecordById/");
        }
    }
}

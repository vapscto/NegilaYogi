using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterMaritalStatusDelegate
    {
        private readonly object resource;
       // private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_Master_Marital_StatusDTO, IVRM_Master_Marital_StatusDTO> COMMM = new CommonDelegate<IVRM_Master_Marital_StatusDTO, IVRM_Master_Marital_StatusDTO>();

        public IVRM_Master_Marital_StatusDTO onloadgetdetails(IVRM_Master_Marital_StatusDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterMaritalStatusFacade/onloadgetdetails");
        }

        public IVRM_Master_Marital_StatusDTO savedetails(IVRM_Master_Marital_StatusDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterMaritalStatusFacade/");
        }
        public IVRM_Master_Marital_StatusDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterMaritalStatusFacade/getRecordById/");
        }
        public IVRM_Master_Marital_StatusDTO deleterec(IVRM_Master_Marital_StatusDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterMaritalStatusFacade/deactivateRecordById/");
        }


    }
}

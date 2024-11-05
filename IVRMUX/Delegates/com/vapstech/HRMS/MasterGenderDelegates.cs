using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterGenderDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_Master_GenderDTO, IVRM_Master_GenderDTO> COMMM = new CommonDelegate<IVRM_Master_GenderDTO, IVRM_Master_GenderDTO>();

        public IVRM_Master_GenderDTO onloadgetdetails(IVRM_Master_GenderDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterGenderFacade/onloadgetdetails");
        }

        public IVRM_Master_GenderDTO savedetails(IVRM_Master_GenderDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterGenderFacade/");
        }
        public IVRM_Master_GenderDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterGenderFacade/getRecordById/");
        }
        public IVRM_Master_GenderDTO deleterec(IVRM_Master_GenderDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterGenderFacade/deactivateRecordById/");
        }


    }
}

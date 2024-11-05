using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterProfessionalTaxDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_ProfessionalTaxDTO, HR_Master_ProfessionalTaxDTO> COMMM = new CommonDelegate<HR_Master_ProfessionalTaxDTO, HR_Master_ProfessionalTaxDTO>();

        public HR_Master_ProfessionalTaxDTO onloadgetdetails(HR_Master_ProfessionalTaxDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterProfessionalTaxFacade/onloadgetdetails");
        }

        public HR_Master_ProfessionalTaxDTO savedetails(HR_Master_ProfessionalTaxDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterProfessionalTaxFacade/");
        }
        public HR_Master_ProfessionalTaxDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterProfessionalTaxFacade/getRecordById/");
        }
        public HR_Master_ProfessionalTaxDTO deleterec(HR_Master_ProfessionalTaxDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterProfessionalTaxFacade/deactivateRecordById/");
        }

       

       

    }
}

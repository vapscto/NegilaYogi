using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterIncomeTaxDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_IncomeTax_DetailsDTO, HR_Master_IncomeTax_DetailsDTO> COMMM = new CommonDelegate<HR_Master_IncomeTax_DetailsDTO, HR_Master_IncomeTax_DetailsDTO>();

        public HR_Master_IncomeTax_DetailsDTO onloadgetdetails(HR_Master_IncomeTax_DetailsDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterIncomeTaxDetailsFacade/onloadgetdetails");
        }

        public HR_Master_IncomeTax_DetailsDTO savedetails(HR_Master_IncomeTax_DetailsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterIncomeTaxDetailsFacade/");
        }
        public HR_Master_IncomeTax_DetailsDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterIncomeTaxDetailsFacade/getRecordById/");
        }
        public HR_Master_IncomeTax_DetailsDTO deleterec(HR_Master_IncomeTax_DetailsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterIncomeTaxDetailsFacade/deactivateRecordById/");
        }

    }
}

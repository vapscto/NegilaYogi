using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterIncomeTaxDetailsCessDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_IncomeTax_Details_CessDTO, HR_Master_IncomeTax_Details_CessDTO> COMMM = new CommonDelegate<HR_Master_IncomeTax_Details_CessDTO, HR_Master_IncomeTax_Details_CessDTO>();

        public HR_Master_IncomeTax_Details_CessDTO onloadgetdetails(HR_Master_IncomeTax_Details_CessDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterIncomeTaxDetailsCessFacade/onloadgetdetails");
        }

        public HR_Master_IncomeTax_Details_CessDTO savedetails(HR_Master_IncomeTax_Details_CessDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterIncomeTaxDetailsCessFacade/");
        }
        public HR_Master_IncomeTax_Details_CessDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterIncomeTaxDetailsCessFacade/getRecordById/");
        }
        public HR_Master_IncomeTax_Details_CessDTO deleterec(HR_Master_IncomeTax_Details_CessDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterIncomeTaxDetailsCessFacade/deactivateRecordById/");
        }

    }
}

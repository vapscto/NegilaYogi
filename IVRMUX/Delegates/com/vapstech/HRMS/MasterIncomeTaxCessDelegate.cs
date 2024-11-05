using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterIncomeTaxCessDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_IncomeTax_CessDTO, HR_Master_IncomeTax_CessDTO> COMMM = new CommonDelegate<HR_Master_IncomeTax_CessDTO, HR_Master_IncomeTax_CessDTO>();

        public HR_Master_IncomeTax_CessDTO onloadgetdetails(HR_Master_IncomeTax_CessDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterIncomeTaxCessFacade/onloadgetdetails");
        }

        public HR_Master_IncomeTax_CessDTO savedetails(HR_Master_IncomeTax_CessDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterIncomeTaxCessFacade/");
        }
        public HR_Master_IncomeTax_CessDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterIncomeTaxCessFacade/getRecordById/");
        }
        public HR_Master_IncomeTax_CessDTO deleterec(HR_Master_IncomeTax_CessDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterIncomeTaxCessFacade/deactivateRecordById/");
        }

       

       

    }
}

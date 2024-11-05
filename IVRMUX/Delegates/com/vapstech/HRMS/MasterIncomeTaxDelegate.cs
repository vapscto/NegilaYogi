using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterIncomeTaxDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_IncomeTaxDTO, HR_Master_IncomeTaxDTO> COMMM = new CommonDelegate<HR_Master_IncomeTaxDTO, HR_Master_IncomeTaxDTO>();

        public HR_Master_IncomeTaxDTO onloadgetdetails(HR_Master_IncomeTaxDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterIncomeTaxFacade/onloadgetdetails");
        }

        public HR_Master_IncomeTaxDTO savedetails(HR_Master_IncomeTaxDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterIncomeTaxFacade/");
        }
        public HR_Master_IncomeTaxDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterIncomeTaxFacade/getRecordById/");
        }
        public HR_Master_IncomeTaxDTO deleterec(HR_Master_IncomeTaxDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterIncomeTaxFacade/deactivateRecordById/");
        }

    }
}

using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterBankDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_BankDeatilsDTO, HR_Master_BankDeatilsDTO> COMMM = new CommonDelegate<HR_Master_BankDeatilsDTO, HR_Master_BankDeatilsDTO>();

        public HR_Master_BankDeatilsDTO onloadgetdetails(HR_Master_BankDeatilsDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterBankFacade/onloadgetdetails");
        }

        public HR_Master_BankDeatilsDTO savedetails(HR_Master_BankDeatilsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterBankFacade/");
        }
        public HR_Master_BankDeatilsDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterBankFacade/getRecordById/");
        }
        public HR_Master_BankDeatilsDTO deleterec(HR_Master_BankDeatilsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterBankFacade/deactivateRecordById/");
        }

    }
}

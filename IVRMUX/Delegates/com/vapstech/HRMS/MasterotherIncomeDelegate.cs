using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
   
    public class MasterotherIncomeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_master_otherIncomeDTO, HR_master_otherIncomeDTO> COMMM = new CommonDelegate<HR_master_otherIncomeDTO, HR_master_otherIncomeDTO>();

        public HR_master_otherIncomeDTO onloadgetdetails(HR_master_otherIncomeDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterotherIncomeFacade/onloadgetdetails");
        }

        public HR_master_otherIncomeDTO savedetails(HR_master_otherIncomeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterotherIncomeFacade/");
        }
        public HR_master_otherIncomeDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterotherIncomeFacade/getRecordById/");
        }
        public HR_master_otherIncomeDTO deleterec(HR_master_otherIncomeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterotherIncomeFacade/deactivateRecordById/");
        }

    }
}

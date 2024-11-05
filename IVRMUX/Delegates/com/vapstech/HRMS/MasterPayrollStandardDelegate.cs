using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterPayrollStandardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_ConfigurationDTO, HR_ConfigurationDTO> COMMM = new CommonDelegate<HR_ConfigurationDTO, HR_ConfigurationDTO>();

        public HR_ConfigurationDTO onloadgetdetails(HR_ConfigurationDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterPayrollStandardFacade/onloadgetdetails");
        }

        public HR_ConfigurationDTO Onchangedetails(HR_ConfigurationDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterPayrollStandardFacade/Onchangedetails");
        }
        public HR_ConfigurationDTO savedetails(HR_ConfigurationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterPayrollStandardFacade/");
        }
        public HR_ConfigurationDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterPayrollStandardFacade/getRecordById/");
        }
        public HR_ConfigurationDTO deleterec(HR_ConfigurationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterPayrollStandardFacade/deactivateRecordById/");
        }
    }
}

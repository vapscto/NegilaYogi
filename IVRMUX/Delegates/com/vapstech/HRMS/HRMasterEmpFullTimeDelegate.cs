using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HRMasterEmpFullTimeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAACHRMasterEmpFullTimeDTO, NAACHRMasterEmpFullTimeDTO> COMMM = new CommonDelegate<NAACHRMasterEmpFullTimeDTO, NAACHRMasterEmpFullTimeDTO>();

        public NAACHRMasterEmpFullTimeDTO getalldetails(NAACHRMasterEmpFullTimeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HRMasterEmpFullTimeFacade/getalldetails/");
        }

        public NAACHRMasterEmpFullTimeDTO savedata(NAACHRMasterEmpFullTimeDTO data)
        {
            return COMMM.POSTDataHRMS(data, "HRMasterEmpFullTimeFacade/savedata/");
        }

        public NAACHRMasterEmpFullTimeDTO editRecord(NAACHRMasterEmpFullTimeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HRMasterEmpFullTimeFacade/editRecord/");
        }

        public NAACHRMasterEmpFullTimeDTO ActiveDeactiveRecord(NAACHRMasterEmpFullTimeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HRMasterEmpFullTimeFacade/ActiveDeactiveRecord/");
        }
    }
}

using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class AlertReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterEmployeeDTO, MasterEmployeeDTO> COMMM = new CommonDelegate<MasterEmployeeDTO, MasterEmployeeDTO>();

        public MasterEmployeeDTO getAlertReport(MasterEmployeeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "AlertReportFacade/getAlertReport/");
        }

    }
}

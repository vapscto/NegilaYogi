using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Student;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.IVRM;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.IVRM
{
    public class PortalMonthEndReportDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<PortalMonthEndReportDTO, PortalMonthEndReportDTO> COMMM = new CommonDelegate<PortalMonthEndReportDTO, PortalMonthEndReportDTO>();


        public PortalMonthEndReportDTO getloaddata(PortalMonthEndReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "PortalMonthEndReportFacade/getloaddata/");
        }
        public PortalMonthEndReportDTO getmonthreport(PortalMonthEndReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "PortalMonthEndReportFacade/getmonthreport/");
        }    
     

    }
}

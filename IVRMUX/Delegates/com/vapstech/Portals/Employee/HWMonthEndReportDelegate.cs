using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Employee
{
    public class HWMonthEndReportDelegate
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HomeWorkUploadDTO, HomeWorkUploadDTO> COMMM = new CommonDelegate<HomeWorkUploadDTO, HomeWorkUploadDTO>();

        public HomeWorkUploadDTO getdata123(HomeWorkUploadDTO data)
        {
            return COMMM.POSTPORTALData(data, "HWMonthEndReportFacade/getalldetails123/");
        }

        public HomeWorkUploadDTO getreport(HomeWorkUploadDTO data)
        {
            return COMMM.POSTPORTALData(data, "HWMonthEndReportFacade/getreport/");
        }

       
    }
}

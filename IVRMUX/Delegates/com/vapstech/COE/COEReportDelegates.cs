using CommonLibrary;
using PreadmissionDTOs.com.vaps.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.COE
{
    public class COEReportDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<COEReportDTO, COEReportDTO> COMMM = new CommonDelegate<COEReportDTO, COEReportDTO>();
        public COEReportDTO getdetails(int id)
        {
            return COMMM.GetDataByIdCOE(id, "COEReportFacade/getdata/");
        }
        public COEReportDTO getReport(COEReportDTO dto)
        {
            return COMMM.POSTDataCOE(dto, "COEReportFacade/");
        }

        public COEReportDTO mothreport(COEReportDTO dto)
        {
            return COMMM.POSTDataCOE(dto, "COEReportFacade/mothreport");
        }
    }
}

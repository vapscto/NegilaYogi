using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    public class YearEndReportDelegate
    {
        CommonDelegate<YearEndReportDTO, YearEndReportDTO> COMSPRT = new CommonDelegate<YearEndReportDTO, YearEndReportDTO>();

        public YearEndReportDTO loadDrpDwn(YearEndReportDTO data)
        {
            return COMSPRT.POSTDataSports(data, "YearEndReportFacade/loadDrpDwn/");
        }
        public YearEndReportDTO getReport(YearEndReportDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "YearEndReportFacade/getReport/");
        }
        public YearEndReportDTO getReportGraph(YearEndReportDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "YearEndReportFacade/getReportGraph/");
        }
        
    }
}

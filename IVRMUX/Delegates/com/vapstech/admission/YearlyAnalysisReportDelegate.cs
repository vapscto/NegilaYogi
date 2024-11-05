using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class YearlyAnalysisReportDelegate
    {
        CommonDelegate<YearlyAnalysisReportDTO, YearlyAnalysisReportDTO> _comm = new CommonDelegate<YearlyAnalysisReportDTO, YearlyAnalysisReportDTO>();
        public YearlyAnalysisReportDTO loaddata(YearlyAnalysisReportDTO data)
        {
            return _comm.POSTDataADM(data, "YearlyAnalysisReportFacade/loaddata");
        }
        public YearlyAnalysisReportDTO report(YearlyAnalysisReportDTO data)
        {
            return _comm.POSTDataADM(data, "YearlyAnalysisReportFacade/report");
        }
    }
}

using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class GatePassReportDelegate
    {
        CommonDelegate<GatePassReportDTO, GatePassReportDTO> COMVISITOR = new CommonDelegate<GatePassReportDTO, GatePassReportDTO>();

        public GatePassReportDTO report(GatePassReportDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "GatePassReportFacade/report/");
        }

        public GatePassReportDTO loaddata(GatePassReportDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "GatePassReportFacade/loaddata/");
        }
        public GatePassReportDTO reportforMobile(GatePassReportDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "GatePassReportFacade/reportforMobile/");
        }

    }
}

using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class GetVisitorReportDelegate
    {
        CommonDelegate<GetVisitorReportDTO, GetVisitorReportDTO> COMVISITOR = new CommonDelegate<GetVisitorReportDTO, GetVisitorReportDTO>();

        public GetVisitorReportDTO report(GetVisitorReportDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "GetVisitorReportFacade/report/");
        }

    }
}

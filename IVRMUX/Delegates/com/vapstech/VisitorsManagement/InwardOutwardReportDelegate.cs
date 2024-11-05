using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class InwardOutwardReportDelegate
    {
        CommonDelegate<InwardOutwardReportDTO, InwardOutwardReportDTO> COMVISITOR = new CommonDelegate<InwardOutwardReportDTO, InwardOutwardReportDTO>();

        public InwardOutwardReportDTO report(InwardOutwardReportDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "InwardOutwardReportFacade/report/");
        }
       
    }
}

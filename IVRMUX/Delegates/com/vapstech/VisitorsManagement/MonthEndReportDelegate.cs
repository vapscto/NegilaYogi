using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class MonthEndReportDelegate
    {

        CommonDelegate<VisitorsMonthEndReport_DTO, VisitorsMonthEndReport_DTO> COMSPRT = new CommonDelegate<VisitorsMonthEndReport_DTO, VisitorsMonthEndReport_DTO>();

        public VisitorsMonthEndReport_DTO getdeatils(VisitorsMonthEndReport_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "MonthEndReportFacade/getdeatils/");
        }
        public VisitorsMonthEndReport_DTO GetReport(VisitorsMonthEndReport_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "MonthEndReportFacade/GetReport/");
        }

    }
}

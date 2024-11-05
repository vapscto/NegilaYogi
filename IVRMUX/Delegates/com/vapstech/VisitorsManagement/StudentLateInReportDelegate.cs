using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class StudentLateInReportDelegate
    {
        CommonDelegate<LateInStudent_DTO, LateInStudent_DTO> COMVISITOR = new CommonDelegate<LateInStudent_DTO, LateInStudent_DTO>();

        public LateInStudent_DTO loaddata(LateInStudent_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "StudentLateInReportFacade/loaddata/");
        }
        public LateInStudent_DTO getReport(LateInStudent_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "StudentLateInReportFacade/getReport/");
        }


    }
}

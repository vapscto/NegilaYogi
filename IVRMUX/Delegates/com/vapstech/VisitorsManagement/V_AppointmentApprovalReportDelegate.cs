using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class V_AppointmentApprovalReportDelegate
    {
        CommonDelegate<V_AppointmentApprovalReport_DTO, V_AppointmentApprovalReport_DTO> COMVISITOR = new CommonDelegate<V_AppointmentApprovalReport_DTO, V_AppointmentApprovalReport_DTO>();

        public V_AppointmentApprovalReport_DTO loaddata(V_AppointmentApprovalReport_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "V_AppointmentApprovalReportFacade/loaddata/");
        }
        public V_AppointmentApprovalReport_DTO loaddatatoday(V_AppointmentApprovalReport_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "V_AppointmentApprovalReportFacade/loaddatatoday/");
        }

        public V_AppointmentApprovalReport_DTO report(V_AppointmentApprovalReport_DTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "V_AppointmentApprovalReportFacade/report/");
        }
    }
}

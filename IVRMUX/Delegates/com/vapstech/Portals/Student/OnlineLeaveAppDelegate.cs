using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Student
{
    public class OnlineLeaveAppDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OnlineLeaveApp_DTO, OnlineLeaveApp_DTO> COMMM = new CommonDelegate<OnlineLeaveApp_DTO, OnlineLeaveApp_DTO>();
        CommonDelegate<TransferCertificate_DTO, TransferCertificate_DTO> COMMTC = new CommonDelegate<TransferCertificate_DTO, TransferCertificate_DTO>();

        public OnlineLeaveApp_DTO getdetails(OnlineLeaveApp_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "OnlineLeaveAppFacade/getdetails/");
        }

        public OnlineLeaveApp_DTO leaveapply(OnlineLeaveApp_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "OnlineLeaveAppFacade/leaveapply/");
        }

        public OnlineLeaveApp_DTO editdata(OnlineLeaveApp_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "OnlineLeaveAppFacade/editdata/");
        }

        public OnlineLeaveApp_DTO leaveApproved(OnlineLeaveApp_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "OnlineLeaveAppFacade/leaveApproved/");
        }

        public OnlineLeaveApp_DTO leaveRejected(OnlineLeaveApp_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "OnlineLeaveAppFacade/leaveRejected/");
        }

        public OnlineLeaveApp_DTO deactiveY(OnlineLeaveApp_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "OnlineLeaveAppFacade/deactiveY/");
        }

        public OnlineLeaveApp_DTO cancellationRecord(OnlineLeaveApp_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "OnlineLeaveAppFacade/cancellationRecord/");
        }
        public OnlineLeaveApp_DTO getdate_sla(OnlineLeaveApp_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "OnlineLeaveAppFacade/getdate_sla/");
        }
        public OnlineLeaveApp_DTO getsection(OnlineLeaveApp_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "OnlineLeaveAppFacade/getsection/");
        }
         public OnlineLeaveApp_DTO getstudent(OnlineLeaveApp_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "OnlineLeaveAppFacade/getstudent/");
        }
         public OnlineLeaveApp_DTO get_leave_Report(OnlineLeaveApp_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "OnlineLeaveAppFacade/get_leave_Report/");
        }
          public TransferCertificate_DTO get_TC_Report(TransferCertificate_DTO sddto)
        {
            return COMMTC.POSTPORTALData(sddto, "OnlineLeaveAppFacade/get_TC_Report/");
        }
        
    }
}

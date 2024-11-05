using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.LeaveManagement
{
    public class LeaveApprovalDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<LeaveCreditDTO, LeaveCreditDTO> COMMM = new CommonDelegate<LeaveCreditDTO, LeaveCreditDTO>();

        
        private readonly object resource;
        private readonly string serviceBaseUrl;
       
        CommonDelegate<LeaveCreditDTO, LeaveCreditDTO> COMFRNT = new CommonDelegate<LeaveCreditDTO, LeaveCreditDTO>();

        public LeaveCreditDTO getApprovalStatus(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/getApprovalStatus/");
        }   
        public LeaveCreditDTO get_status(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/get_status/");
        }
        public LeaveCreditDTO reject_status(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/reject_status/");
        }

        public LeaveCreditDTO getApprovedLeave(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/getApprovedLeave/");
        }

        public LeaveCreditDTO Viewleavebalancehistory(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/Viewleavebalancehistory/");
        }

        public LeaveCreditDTO getRequestStatus(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/getRequestStatus/");
        }
        public LeaveCreditDTO get_approvestatus(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/get_approvestatus/");
        }

        //periodwiseapproval////////////////////////////////////////////////////////////
        public LeaveCreditDTO getperiodApprovalStatus(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/getperiodApprovalStatus/");
        }
        public LeaveCreditDTO periodleavestatus(LeaveCreditDTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveApprovalFacade/periodleavestatus/");
        }
    }
}

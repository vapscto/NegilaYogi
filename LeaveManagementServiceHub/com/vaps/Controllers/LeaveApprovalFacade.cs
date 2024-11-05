using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class LeaveApprovalFacade : Controller
    {
        public LeaveApprovalInterface _leave;
        public LeaveApprovalFacade(LeaveApprovalInterface _leavec)
        {
            _leave = _leavec;
        }

        [HttpPost]
        [Route("getApprovalStatus")]
        public LeaveCreditDTO getApprovalStatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.getApprovalStatus(data);
        }

        [Route("get_status")]
        public Task<LeaveCreditDTO> get_status([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_status(data);
        }

        [Route("reject_status")]
        public Task<LeaveCreditDTO> reject_status([FromBody] LeaveCreditDTO data)
        {
            return _leave.reject_status(data);
        }

        [HttpPost]
        [Route("getApprovedLeave")]
        public LeaveCreditDTO getApprovedLeave([FromBody] LeaveCreditDTO data)
        {
            return _leave.getApprovedLeave(data);
        }


        [Route("Viewleavebalancehistory")]
        public LeaveCreditDTO Viewleavebalancehistory([FromBody] LeaveCreditDTO data)
        {
            return _leave.Viewleavebalancehistory(data);
        }

        //Onduty Approval
        [Route("getRequestStatus")]
        public LeaveCreditDTO getRequestStatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.getRequestStatus(data);
        }

        [Route("get_approvestatus")]
        public Task<LeaveCreditDTO> get_approvestatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_approvestatusAsync(data);
        }
        /////periodwiseapproval////////////////////////////////////////////////////////////
        
         [Route("getperiodApprovalStatus")]
        public LeaveCreditDTO getperiodApprovalStatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.getperiodApprovalStatus(data);
        }

        [Route("periodleavestatus")]
        public LeaveCreditDTO periodleavestatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.periodleavestatus(data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SMSApprovalTransactionFacadeController : Controller
    {
        SMSApprovalTransactionInterface _SendSms;

        public SMSApprovalTransactionFacadeController(SMSApprovalTransactionInterface SendSms)
        {
            _SendSms = SendSms;
        }

        [HttpPost]
        [Route("Getdetails")]
        public SMSMasterApprovalDTO Getdetails([FromBody] SMSMasterApprovalDTO data)
        {
            return _SendSms.Getdetails(data);
        }

        [HttpPost]
        [Route("editdata")]
        public SMSMasterApprovalDTO editdata([FromBody] SMSMasterApprovalDTO data)
        {
            return _SendSms.editdata(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public SMSMasterApprovalDTO deactivate([FromBody] SMSMasterApprovalDTO data)
        {
            return _SendSms.deactivate(data);

        }
        [HttpPost]
        [Route("GetAttendence")]
        public SMSMasterApprovalDTO GetAttendence([FromBody] SMSMasterApprovalDTO data)
        {


            return _SendSms.GetAttendence(data);

        }

        [HttpPost]
        [Route("savedetails")]
        public SMSMasterApprovalDTO savedetails([FromBody] SMSMasterApprovalDTO data)
        {
            
            return _SendSms.savedetails(data);

        }
        [HttpPost]
        [Route("saveapprove")]
        public SMSMasterApprovalDTO saveapprove([FromBody] SMSMasterApprovalDTO data)
        {
            
            return _SendSms.saveapprove(data);

        }
        [HttpPost]
        [Route("rejectsms")]
        public SMSMasterApprovalDTO rejectsms([FromBody] SMSMasterApprovalDTO data)
        {
            
            return _SendSms.rejectsms(data);

        }


    }
}

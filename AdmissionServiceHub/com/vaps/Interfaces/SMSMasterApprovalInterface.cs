using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;


//PreadmissionDTOs.com.vaps.admission


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface SMSMasterApprovalInterface
    {
        SMSMasterApprovalDTO Getdetails(SMSMasterApprovalDTO data);
        SMSMasterApprovalDTO editdata(SMSMasterApprovalDTO data);
        SMSMasterApprovalDTO deactivate(SMSMasterApprovalDTO data);
        SMSMasterApprovalDTO GetAttendence(SMSMasterApprovalDTO data);
        SMSMasterApprovalDTO savedetails(SMSMasterApprovalDTO data);

    }
}

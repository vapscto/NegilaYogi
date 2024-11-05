using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class External_Training_Approval_Delegate
    {
        CommonDelegate<External_Training_ApprovalDTO, External_Training_ApprovalDTO> _com = new CommonDelegate<External_Training_ApprovalDTO, External_Training_ApprovalDTO>();

        public External_Training_ApprovalDTO onloaddata(External_Training_ApprovalDTO data)
        {
            return _com.POSTVMS(data, "External_Training_ApprovalFacade/onloaddata");
        }
        public External_Training_ApprovalDTO approvalstatus(External_Training_ApprovalDTO data)
        {
            return _com.POSTVMS(data, "External_Training_ApprovalFacade/approvalstatus");
        }
        public External_Training_ApprovalDTO deactiveY(External_Training_ApprovalDTO data)
        {
            return _com.POSTVMS(data, "External_Training_ApprovalFacade/deactiveY");
        }
        public External_Training_ApprovalDTO trainingdetails(External_Training_ApprovalDTO data)
        {
            return _com.POSTVMS(data, "External_Training_ApprovalFacade/trainingdetails");
        }
    }
}


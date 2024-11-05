using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;


namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class SMSApprovalTransactionDelegate
    {
        CommonDelegate<SMSMasterApprovalDTO, SMSMasterApprovalDTO> comm = new CommonDelegate<SMSMasterApprovalDTO, SMSMasterApprovalDTO>();

        public SMSMasterApprovalDTO Getdetails(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSApprovalTransactionFacade/Getdetails/");
        }

        public SMSMasterApprovalDTO editdata(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSApprovalTransactionFacade/editdata/");
        }
        public SMSMasterApprovalDTO deactivate(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSApprovalTransactionFacade/deactivate/");
        }
        public SMSMasterApprovalDTO GetAttendence(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSApprovalTransactionFacade/GetAttendence/");
        }
        public SMSMasterApprovalDTO savedetails(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSApprovalTransactionFacade/savedetails/");
        }
        public SMSMasterApprovalDTO saveapprove(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSApprovalTransactionFacade/saveapprove/");
        }
          public SMSMasterApprovalDTO rejectsms(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSApprovalTransactionFacade/rejectsms/");
        }
        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;


namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class SMSMasterApprovalDelegate
    {
        CommonDelegate<SMSMasterApprovalDTO, SMSMasterApprovalDTO> comm = new CommonDelegate<SMSMasterApprovalDTO, SMSMasterApprovalDTO>();

        public SMSMasterApprovalDTO Getdetails(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSMasterApprovalFacade/Getdetails/");
        }

        public SMSMasterApprovalDTO editdata(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSMasterApprovalFacade/editdata/");
        }
        public SMSMasterApprovalDTO deactivate(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSMasterApprovalFacade/deactivate/");
        }
        public SMSMasterApprovalDTO GetAttendence(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSMasterApprovalFacade/GetAttendence/");
        }
        public SMSMasterApprovalDTO savedetails(SMSMasterApprovalDTO data)
        {
            return comm.POSTDataADM(data, "SMSMasterApprovalFacade/savedetails/");
        }
        

    }
}

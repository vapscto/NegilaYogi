using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{ 
   public class SMSMail_HeaderDelegate
    {
        CommonDelegate<SMSMail_HeaderDTO, SMSMail_HeaderDTO> comm = new CommonDelegate<SMSMail_HeaderDTO, SMSMail_HeaderDTO>();
        public SMSMail_HeaderDTO getdata(SMSMail_HeaderDTO data)
        {
            return comm.POSTDataADM(data, "SMSMail_HeaderFacade/getdata/");
        }
        public SMSMail_HeaderDTO getalldetails(SMSMail_HeaderDTO data)
        {
            return comm.POSTDataADM(data, "SMSMail_HeaderFacade/getalldetails/");
        }
        public SMSMail_HeaderDTO edittab1(SMSMail_HeaderDTO data)
        {
            return comm.POSTDataADM(data, "SMSMail_HeaderFacade/edittab1");
        }
        public SMSMail_HeaderDTO delete(SMSMail_HeaderDTO data)
        {
            return comm.POSTDataADM(data, "SMSMail_HeaderFacade/delete");
        }
        
    }
}

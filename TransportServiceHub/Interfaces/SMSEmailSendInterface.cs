using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Transport;

namespace TransportServiceHub.Interfaces
{
   public interface SMSEmailSendInterface
    {
        SMSEmailSendDTO getdata(int id);
        SMSEmailSendDTO Getreportdetails(SMSEmailSendDTO data);
        Task<SMSEmailSendDTO> smssend(SMSEmailSendDTO data);
        Task<SMSEmailSendDTO> sendWhatsAppCall(SMSEmailSendDTO data);
        SMSEmailSendDTO emailsend(SMSEmailSendDTO data);
    }
}

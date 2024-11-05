using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Transport;

namespace TransportServiceHub.Interfaces
{
    public interface UsrPwdInterface
    {
        SMSEmailSendDTO getdata(int id);
        SMSEmailSendDTO Getreportdetails(SMSEmailSendDTO data);
        SMSEmailSendDTO creusrnme(SMSEmailSendDTO data);
        SMSEmailSendDTO emailsend(SMSEmailSendDTO data);
    }
}

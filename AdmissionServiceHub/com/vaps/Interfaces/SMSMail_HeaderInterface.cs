using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
     public interface SMSMail_HeaderInterface
     {
        SMSMail_HeaderDTO getdata(SMSMail_HeaderDTO data);
        SMSMail_HeaderDTO getalldetails(SMSMail_HeaderDTO data);
        SMSMail_HeaderDTO edittab1(SMSMail_HeaderDTO data);
        SMSMail_HeaderDTO delete(SMSMail_HeaderDTO data);
        
     }
}

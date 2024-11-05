using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
  public  interface SmsEmailSettingInterface
    {
        SMSEmailSettingDTO getallDetails(SMSEmailSettingDTO smsdto);
        SMSEmailSettingDTO getmodulePage(SMSEmailSettingDTO page);
        SMSEmailSettingDTO getHeader(SMSEmailSettingDTO header);
        SmsEmailDTO save(SmsEmailDTO savedata);
        SmsEmailDTO getdetails(int id);
        SmsEmailDTO deleterec(int delId);
        SMS_MAIL_PARAMETER_DTO getParameter(int headerId);
        SmsEmailDTO activedeactivesms(SmsEmailDTO data);
        SmsEmailDTO activedeactiveemail(SmsEmailDTO data);
        SmsEmailDTO viewtempate(SmsEmailDTO data);



    }
}

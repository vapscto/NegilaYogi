using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class UsrPwdDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<SMSEmailSendDTO, SMSEmailSendDTO> comml = new CommonDelegate<SMSEmailSendDTO, SMSEmailSendDTO>();

        public SMSEmailSendDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "SMSEmailSendFacade/getdata/");
        }

        public SMSEmailSendDTO Getreportdetails(SMSEmailSendDTO data)
        {
            return comml.POSTDataTransport(data, "UsrPwdFacade/Getreportdetails/");
        }

        public SMSEmailSendDTO createusername(SMSEmailSendDTO data)
        {
            return comml.POSTDataTransport(data, "UsrPwdFacade/createusrnme/");
        }
        public SMSEmailSendDTO emailsend(SMSEmailSendDTO data)
        {
            return comml.POSTDataTransport(data, "SMSEmailSendFacade/emailsend/");
        }

    }
}

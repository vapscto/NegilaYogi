using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.BirthDay;
using PreadmissionDTOs.com.vaps.College.BirthDay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.BirthDay
{
    public class ClgBirthdayDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgBirthDayDTO, ClgBirthDayDTO> _ClgDelegate = new CommonDelegate<ClgBirthDayDTO, ClgBirthDayDTO>();

        public ClgBirthDayDTO getloaddata(ClgBirthDayDTO data)
        {
            return _ClgDelegate.POSTDataBirthday(data, "BirthdayClgFacade/getloaddata/");
        }
        public ClgBirthDayDTO radiochange(ClgBirthDayDTO data)
        {
            return _ClgDelegate.POSTDataBirthday(data, "BirthdayClgFacade/radiochange/");
        }
        public ClgBirthDayDTO sendmsg(ClgBirthDayDTO data)
        {
            return _ClgDelegate.POSTDataBirthday(data, "BirthdayClgFacade/sendmsg/");
        }

        
    }
}


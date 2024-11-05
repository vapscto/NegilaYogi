using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.BirthDay;
using PreadmissionDTOs.com.vaps.College.BirthDay;
//using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.BirthDay
{
    public class BdayMonthEndReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgBirthDayDTO, ClgBirthDayDTO> _ClgDelegate = new CommonDelegate<ClgBirthDayDTO, ClgBirthDayDTO>();

        public ClgBirthDayDTO getloaddata(ClgBirthDayDTO data)
        {
            return _ClgDelegate.POSTClgBirthday(data, "BdayMonthEndReportFacade/getloaddata/");
        }
        public ClgBirthDayDTO getmonthreport(ClgBirthDayDTO data)
        {
            return _ClgDelegate.POSTClgBirthday(data, "BdayMonthEndReportFacade/getmonthreport/");
        }



    }
}


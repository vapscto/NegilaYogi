using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Student;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Student
{
    public class ClassWorkDownloadDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_ClassWorkDTO, IVRM_ClassWorkDTO> COMMM = new CommonDelegate<IVRM_ClassWorkDTO, IVRM_ClassWorkDTO>();
        public IVRM_ClassWorkDTO getloaddata(IVRM_ClassWorkDTO data)
        {     
            return COMMM.POSTPORTALData(data, "ClassWorkDownloadFacade/getloaddata/");
        }
        public IVRM_ClassWorkDTO getwork(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "ClassWorkDownloadFacade/getwork/");
        }

    }
}

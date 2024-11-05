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
using PreadmissionDTOs.com.vaps.OnlineProgram;


namespace IVRMUX.Delegates.NAAC.OnlineProgram
{
    public class CompletedEventDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OnlineProgramDTO, OnlineProgramDTO> COMMM = new CommonDelegate<OnlineProgramDTO, OnlineProgramDTO>();
        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "CompletedEventFacade/getloaddata/");
        }

        public OnlineProgramDTO Savedata(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "CompletedEventFacade/Savedata/");
        }
        public OnlineProgramDTO getdetails(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "CompletedEventFacade/getdetails/");
        }
        public OnlineProgramDTO deactivate(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "CompletedEventFacade/deactivate/");
        }

    }
}

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
    public class ConferenceDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OnlineProgramDTO, OnlineProgramDTO> COMMM = new CommonDelegate<OnlineProgramDTO, OnlineProgramDTO>();
     //   CommonDelegate<Edit_OnlineProgramDTO, Edit_OnlineProgramDTO> COMMMN = new CommonDelegate<Edit_OnlineProgramDTO, Edit_OnlineProgramDTO>();
        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ConferenceDetailsFacade/getloaddata/");
        }

        public OnlineProgramDTO Savedata(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ConferenceDetailsFacade/Savedata/");
        }
        public OnlineProgramDTO getdetails(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ConferenceDetailsFacade/getdetails/");
        }
        public OnlineProgramDTO delete(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ConferenceDetailsFacade/delete/");
        }

        public OnlineProgramDTO viewuploadflies(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ConferenceDetailsFacade/viewuploadflies/");
        }
        

    }
}

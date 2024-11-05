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
    public class YearlyProgramDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OnlineProgramDTO, OnlineProgramDTO> COMMM = new CommonDelegate<OnlineProgramDTO, OnlineProgramDTO>();
        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "YearlyProgramFacade/getloaddata/");
        }

        public OnlineProgramDTO Savedata(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "YearlyProgramFacade/Savedata/");
        }

        public OnlineProgramDTO removeNewsiblinguest(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "YearlyProgramFacade/removeNewsiblinguest/");
        }


        public OnlineProgramDTO editguest(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "YearlyProgramFacade/editguest/");
        }
        public OnlineProgramDTO getdetails(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "YearlyProgramFacade/getdetails/");
        }
        public OnlineProgramDTO delete(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "YearlyProgramFacade/delete/");
        }

        public OnlineProgramDTO viewuploadflies(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "YearlyProgramFacade/viewuploadflies/");
        }
        

    }
}

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
    public class ProgramMasterDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OnlineProgramDTO, OnlineProgramDTO> COMMM = new CommonDelegate<OnlineProgramDTO, OnlineProgramDTO>();
        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramMasterFacade/getloaddata/");
        }

        public OnlineProgramDTO savedatatype(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramMasterFacade/savedatatype/");
        }
        public OnlineProgramDTO savedatalevel(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramMasterFacade/savedatalevel/");
        }
        public OnlineProgramDTO deactivelevel(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramMasterFacade/deactivelevel/");
        }
        public OnlineProgramDTO editlevel(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramMasterFacade/editlevel/");
        }
        public OnlineProgramDTO edittype(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramMasterFacade/edittype/");
        }
        public OnlineProgramDTO deactivetype(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramMasterFacade/deactivetype/");
        }




    }
}

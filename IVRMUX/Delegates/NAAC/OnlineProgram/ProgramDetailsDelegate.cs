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
    public class ProgramDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OnlineProgramDTO, OnlineProgramDTO> COMMM = new CommonDelegate<OnlineProgramDTO, OnlineProgramDTO>();
        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramDetailsFacade/getloaddata/");
        }

        public OnlineProgramDTO savedetail(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramDetailsFacade/savedetail/");
        }

        public OnlineProgramDTO getalldetailsviewrecords(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramDetailsFacade/getalldetailsviewrecords/");
        }

        public OnlineProgramDTO deleterecord(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramDetailsFacade/deleterecord/");
        }


    }

}


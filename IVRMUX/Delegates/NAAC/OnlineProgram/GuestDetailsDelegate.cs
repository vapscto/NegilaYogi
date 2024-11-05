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
    public class GuestDetailsDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OnlineProgramDTO, OnlineProgramDTO> COMMM = new CommonDelegate<OnlineProgramDTO, OnlineProgramDTO>();
        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "GuestDetailsFacade/getloaddata/");
        }

        public OnlineProgramDTO savedetail(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "GuestDetailsFacade/savedetail/");
        }

        public OnlineProgramDTO getalldetailsviewrecords(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "GuestDetailsFacade/getalldetailsviewrecords/");
        }
        public OnlineProgramDTO getdetails(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "GuestDetailsFacade/getdetails/");
        }
        public OnlineProgramDTO deleterecord(OnlineProgramDTO data)
        {
            return COMMM.naacdetailsbypost(data, "GuestDetailsFacade/deleterecord/");
        }
    }
}

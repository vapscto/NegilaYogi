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
using PreadmissionDTOs.com.vaps.OnlineExam;

namespace corewebapi18072016.Delegates.com.vapstech.OnlineExam
{
    public class OnlineExamConfigDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterQuestionDTO, MasterQuestionDTO> COMMM = new CommonDelegate<MasterQuestionDTO, MasterQuestionDTO>();
        public MasterQuestionDTO getloaddata(MasterQuestionDTO data)
        {     
            return COMMM.POSTData(data, "OnlineExamConfigFacade/getloaddata/");
        }

        //-----------------------1st Tab
        public MasterQuestionDTO savedata(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "OnlineExamConfigFacade/savedetails/");
        }
        public MasterQuestionDTO editQuestion(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "OnlineExamConfigFacade/editQuestion/");
        }
        
        public MasterQuestionDTO Deletedetails(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "OnlineExamConfigFacade/Deletedetails/");
        }
        public MasterQuestionDTO getreport(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "OnlineExamConfigFacade/getreport/");
        }

        //=====================online exam report new
        public MasterQuestionDTO getsection(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "OnlineExamConfigFacade/getsection/");
        }
         public MasterQuestionDTO getonlinereport(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "OnlineExamConfigFacade/getonlinereport/");
        }
        
    }
}

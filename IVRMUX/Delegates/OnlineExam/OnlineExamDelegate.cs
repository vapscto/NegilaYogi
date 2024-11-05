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
    public class OnlineExamDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OnlineExamDTO, OnlineExamDTO> COMMM = new CommonDelegate<OnlineExamDTO, OnlineExamDTO>();
        public OnlineExamDTO getloaddata(OnlineExamDTO data)
        {     
            return COMMM.POSTData(data, "OnlineExamFacade/getloaddata/");
        }

        public OnlineExamDTO getSubjects(OnlineExamDTO data)
        {
            return COMMM.POSTData(data, "OnlineExamFacade/getSubjects/");
        }
        public OnlineExamDTO getQuestion(OnlineExamDTO data)
        {
            return COMMM.POSTData(data, "OnlineExamFacade/getQuestion/");
        }

        public OnlineExamDTO Saveanswer(OnlineExamDTO data)
        {
            return COMMM.POSTData(data, "OnlineExamFacade/Saveanswer/");
        }

        public OnlineExamDTO savedanswers(OnlineExamDTO data)
        {
            return COMMM.POSTData(data, "OnlineExamFacade/savedanswers/");
        }

        public OnlineExamDTO submitexam(OnlineExamDTO data)
        {
            return COMMM.POSTData(data, "OnlineExamFacade/submitexam/");
        }
    }
}

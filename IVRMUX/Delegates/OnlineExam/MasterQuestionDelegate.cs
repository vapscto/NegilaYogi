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
    public class MasterQuestionDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterQuestionDTO, MasterQuestionDTO> COMMM = new CommonDelegate<MasterQuestionDTO, MasterQuestionDTO>();
        public MasterQuestionDTO getloaddata(MasterQuestionDTO data)
        {     
            return COMMM.POSTData(data, "MasterQuestionFacade/getloaddata/");
        }

        //-----------------------1st Tab
        public MasterQuestionDTO savedetails(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionFacade/savedetails/");
        }
         public MasterQuestionDTO viewdocumetns(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionFacade/viewdocumetns/");
        }
         public MasterQuestionDTO deactiveparticulars(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionFacade/deactiveparticulars/");
        }

        //-----------------------2st Tab
        public MasterQuestionDTO savedataclass(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionFacade/savedataclass/");
        }

        

        public MasterQuestionDTO editQuestion(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionFacade/editQuestion/");
        }

        //-----------------------------2nd Tab
        public MasterQuestionDTO savedetails1(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionFacade/savedetails1/");
        }
        public MasterQuestionDTO optionChange(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionFacade/optionChange/");
        }
        public MasterQuestionDTO optiondetails(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionFacade/optiondetails/");
        }
        public MasterQuestionDTO Deletedetails(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionFacade/Deletedetails/");
        }


    }
}

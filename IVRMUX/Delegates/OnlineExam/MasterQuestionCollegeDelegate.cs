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
    public class MasterQuestionCollegeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterQuestionDTO, MasterQuestionDTO> COMMM = new CommonDelegate<MasterQuestionDTO, MasterQuestionDTO>();
        public MasterQuestionDTO getloaddata(MasterQuestionDTO data)
        {     
            return COMMM.POSTData(data, "MasterQuestionCollegeFacade/getloaddata/");
        }

        //-----------------------1st Tab
        public MasterQuestionDTO savedetails(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionCollegeFacade/savedetails/");
        }

        //-----------------------2st Tab
        public MasterQuestionDTO savedataclass(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionCollegeFacade/savedataclass/");
        }

        

        public MasterQuestionDTO editQuestion(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionCollegeFacade/editQuestion/");
        }

        //-----------------------------2nd Tab
        public MasterQuestionDTO savedetails1(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionCollegeFacade/savedetails1/");
        }
        public MasterQuestionDTO optionChange(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionCollegeFacade/optionChange/");
        }
        public MasterQuestionDTO optiondetails(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionCollegeFacade/optiondetails/");
        }
        public MasterQuestionDTO Deletedetails(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionCollegeFacade/Deletedetails/");
        }

        public MasterQuestionDTO selectcourse(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionCollegeFacade/selectcourse/");
        }
        public MasterQuestionDTO selectbran(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionCollegeFacade/selectbran/");
        }
        public MasterQuestionDTO editbranchquestion(MasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "MasterQuestionCollegeFacade/editbranchquestion/");
        }


    }
}

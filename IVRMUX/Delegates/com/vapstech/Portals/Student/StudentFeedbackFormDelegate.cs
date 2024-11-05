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
using PreadmissionDTOs.com.vaps.College.Portals;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals.Student
{
    public class ClgStudentFeedbackFormDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgStudentFeedbackFormDTO, ClgStudentFeedbackFormDTO> COMMM = new CommonDelegate<ClgStudentFeedbackFormDTO, ClgStudentFeedbackFormDTO>();
        public ClgStudentFeedbackFormDTO getloaddata(ClgStudentFeedbackFormDTO data)
        {     
            return COMMM.CLGPortalPOSTData(data, "ClgStudentFeedbackFormFacade/getloaddata/");
        }
        public ClgStudentFeedbackFormDTO savefeedback(ClgStudentFeedbackFormDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentFeedbackFormFacade/savefeedback/");
        }
        public ClgStudentFeedbackFormDTO deactive(ClgStudentFeedbackFormDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentFeedbackFormFacade/deactive/");
        }

        

    }
}

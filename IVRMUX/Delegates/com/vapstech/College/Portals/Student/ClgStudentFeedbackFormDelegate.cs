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

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Student
{
    public class StudentFeedbackFormDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentFeedbackFormDTO, StudentFeedbackFormDTO> COMMM = new CommonDelegate<StudentFeedbackFormDTO, StudentFeedbackFormDTO>();
        public StudentFeedbackFormDTO getloaddata(StudentFeedbackFormDTO data)
        {     
            return COMMM.POSTPORTALData(data, "StudentFeedbackFormFacade/getloaddata/");
        }
        public StudentFeedbackFormDTO savefeedback(StudentFeedbackFormDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentFeedbackFormFacade/savefeedback/");
        }
        public StudentFeedbackFormDTO deactive(StudentFeedbackFormDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentFeedbackFormFacade/deactive/");
        }

        

    }
}

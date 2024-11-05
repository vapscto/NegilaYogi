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
    public class Stu_FeedbackDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Stu_FeedbackDTO, Stu_FeedbackDTO> COMMM = new CommonDelegate<Stu_FeedbackDTO, Stu_FeedbackDTO>();
        public Stu_FeedbackDTO getloaddata(Stu_FeedbackDTO data)
        {     
            return COMMM.POSTPORTALData(data, "Stu_FeedbackFacade/getloaddata/");
        }

        public Stu_FeedbackDTO savecomment(Stu_FeedbackDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "Stu_FeedbackFacade/savecomment/");
        }
        public Stu_FeedbackDTO getexamdetails(Stu_FeedbackDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "Stu_FeedbackFacade/getexamdetails/");
        }
    }
}

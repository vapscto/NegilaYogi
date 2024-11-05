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
    public class CumulativeSubjectDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ExamDTO, ExamDTO> COMMM = new CommonDelegate<ExamDTO, ExamDTO>();
        public ExamDTO getloaddata(ExamDTO data)
        {     
            return COMMM.POSTPORTALData(data, "CumulativeSubjectFacade/getloaddata/");
        }

        public ExamDTO getSubjectsdata(ExamDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "CumulativeSubjectFacade/getSubjectsdata/");
        }
        public ExamDTO getexamdetails(ExamDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "CumulativeSubjectFacade/getexamdetails/");
        }
    }
}

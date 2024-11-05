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
    public class ExamReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ExamDTO, ExamDTO> COMMM = new CommonDelegate<ExamDTO, ExamDTO>();
        public ExamDTO getloaddata(ExamDTO data)
        {     
            return COMMM.POSTPORTALData(data, "ExamReportFacade/getloaddata/");
        }

        public ExamDTO getexamdata(ExamDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "ExamReportFacade/getexamdata/");
        }
        public ExamDTO getSubjects(ExamDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "ExamReportFacade/getSubjects/");
        }

        public ExamDTO getStudentExamDetails(ExamDTO dto)
        {
            return COMMM.POSTPORTALData(dto, "ExamReportFacade/StudentExamDetails/");
        }
    }
}

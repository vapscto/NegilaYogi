using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.HOD
{
    public class HODExamReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ExamDTO, ExamDTO> COMMM = new CommonDelegate<ExamDTO, ExamDTO>();
        public ExamDTO getloaddata(ExamDTO data)
        {     
            return COMMM.POSTPORTALData(data, "StudentHODFacade/getloaddata/");
        }

        public ExamDTO getexamdata(ExamDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentHODFacade/getexamdata/");
        }
        public ExamDTO getexamdetails(ExamDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentHODFacade/getexamdetails/");
        }
        public ExamDTO getsectiondata(ExamDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentHODFacade/getsectiondata/");
        }
        public ExamDTO get_classes(ExamDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentHODFacade/get_classes/");
        }
        public ExamDTO getstudentdata(ExamDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentHODFacade/getstudentdata/");
        }
    }
}

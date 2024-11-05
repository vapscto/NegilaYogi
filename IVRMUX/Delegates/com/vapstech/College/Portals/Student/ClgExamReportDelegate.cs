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
using PreadmissionDTOs.com.vaps.College.Portals.Student;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Student
{
    public class ClgExamReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgExamDTO, ClgExamDTO> COMMM = new CommonDelegate<ClgExamDTO, ClgExamDTO>();
        public ClgExamDTO getloaddata(ClgExamDTO data)
        {     
            return COMMM.CLGPortalPOSTData(data, "ClgExamReportFacade/getloaddata/");
        }

        public ClgExamDTO getexamdata(ClgExamDTO sddto)
        {
            return COMMM.CLGPortalPOSTData(sddto, "ClgExamReportFacade/getexamdata/");
        }
        public ClgExamDTO getSubjects(ClgExamDTO sddto)
        {
            return COMMM.CLGPortalPOSTData(sddto, "ClgExamReportFacade/getSubjects/");
        }

        public ClgExamDTO getStudentExamDetails(ClgExamDTO dto)
        {
            return COMMM.CLGPortalPOSTData(dto, "ClgExamReportFacade/StudentExamDetails/");
        }
    }
}

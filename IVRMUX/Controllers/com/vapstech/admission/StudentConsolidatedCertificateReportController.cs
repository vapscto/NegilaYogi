using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class StudentConsolidatedCertificateReportController : Controller
    {
        StudentConsolidatedCertificateReportDelegate crStr = new StudentConsolidatedCertificateReportDelegate();

        [Route("GetAcademicYear")]
        public StudentConsolidatedCertificateReportDTO GetAcademicYear()
        {
            //int MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentConsolidatedCertificateReportDTO data = new StudentConsolidatedCertificateReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return crStr.GetAcademicYear(data);
        }

        [Route("GetClassDetails")]
        public StudentConsolidateCertificateGetClassParaDTO GetClassDetails([FromBody] StudentConsolidateCertificateGetClassParaDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.GetClassDetails(data);
        }
        [Route("GetSectionDetails")]
        public StudentConsolidateCertificateGetClassParaDTO GetSectionDetails([FromBody] StudentConsolidateCertificateGetClassParaDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.GetSectionDetails(data);
        }
        [Route("GetCertificateDetails")]
        public StudentConsolidateCertificateGetClassParaDTO GetCertificateDetails([FromBody] StudentConsolidateCertificateGetClassParaDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.GetCertificateDetails(data);
        }
        [Route("GetStudentDetails")]
        public StudentConsolidateCertificateGetClassParaDTO GetStudentDetails([FromBody] StudentConsolidateCertificateGetClassParaDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.GetStudentDetails(data);
        }
    }
}
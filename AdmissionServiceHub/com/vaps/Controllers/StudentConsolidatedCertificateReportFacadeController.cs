using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentConsolidatedCertificateReportFacadeController : Controller
    {
        public StudentConsolidatedCertificateReportInterface _ads;

        public StudentConsolidatedCertificateReportFacadeController(StudentConsolidatedCertificateReportInterface adstu)
        {
            _ads = adstu;
        }

        //[HttpGet]
        [Route("GetAcademicYear")]
        public StudentConsolidatedCertificateReportDTO GetAcademicYear([FromBody] StudentConsolidatedCertificateReportDTO Data)
        {
            return _ads.GetAcademicYear(Data);
        }

        [Route("GetClassDetails")]
        public StudentConsolidateCertificateGetClassParaDTO GetClassDetails([FromBody] StudentConsolidateCertificateGetClassParaDTO data)
        {
            return _ads.GetClass(data);
        }
        [Route("GetSectionDetails")]
        public StudentConsolidateCertificateGetClassParaDTO GetSectionDetails([FromBody] StudentConsolidateCertificateGetClassParaDTO data)
        {
            return _ads.GetSectionDetails(data);
        }
        [Route("GetCertificateDetails")]
        public StudentConsolidateCertificateGetClassParaDTO GetCertificateDetails([FromBody] StudentConsolidateCertificateGetClassParaDTO data)
        {
            return _ads.GetCertificateDetails(data);
        }
        [Route("GetStudentDetails")]
        public StudentConsolidateCertificateGetClassParaDTO GetStudentDetails([FromBody] StudentConsolidateCertificateGetClassParaDTO data)
        {
            return _ads.GetStudentDetails(data);
        }
    }
}
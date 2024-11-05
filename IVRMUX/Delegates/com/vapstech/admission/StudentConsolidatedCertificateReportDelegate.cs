using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class StudentConsolidatedCertificateReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentConsolidatedCertificateReportDTO, StudentConsolidatedCertificateReportDTO> COMMM = new CommonDelegate<StudentConsolidatedCertificateReportDTO, StudentConsolidatedCertificateReportDTO>();

        CommonDelegate<StudentConsolidateCertificateGetClassParaDTO, StudentConsolidateCertificateGetClassParaDTO> COMMM1 = new CommonDelegate<StudentConsolidateCertificateGetClassParaDTO, StudentConsolidateCertificateGetClassParaDTO>();
        public StudentConsolidatedCertificateReportDTO GetAcademicYear(StudentConsolidatedCertificateReportDTO Data)
        {
            return COMMM.POSTDataADM(Data, "StudentConsolidatedCertificateReportFacade/GetAcademicYear/");
        }
        public StudentConsolidateCertificateGetClassParaDTO GetClassDetails(StudentConsolidateCertificateGetClassParaDTO data)
        {
            return COMMM1.POSTDataADM(data, "StudentConsolidatedCertificateReportFacade/GetClassDetails/");
        }
        public StudentConsolidateCertificateGetClassParaDTO GetSectionDetails(StudentConsolidateCertificateGetClassParaDTO data)
        {
            return COMMM1.POSTDataADM(data, "StudentConsolidatedCertificateReportFacade/GetSectionDetails/");
        }
        public StudentConsolidateCertificateGetClassParaDTO GetCertificateDetails(StudentConsolidateCertificateGetClassParaDTO data)
        {
            return COMMM1.POSTDataADM(data, "StudentConsolidatedCertificateReportFacade/GetCertificateDetails/");
        }
        public StudentConsolidateCertificateGetClassParaDTO GetStudentDetails(StudentConsolidateCertificateGetClassParaDTO data)
        {
            return COMMM1.POSTDataADM(data, "StudentConsolidatedCertificateReportFacade/GetStudentDetails/");
        }
    }
}

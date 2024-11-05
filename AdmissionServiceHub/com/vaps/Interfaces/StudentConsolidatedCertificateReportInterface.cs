using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudentConsolidatedCertificateReportInterface
    {
        StudentConsolidatedCertificateReportDTO GetAcademicYear(StudentConsolidatedCertificateReportDTO Data);
        StudentConsolidateCertificateGetClassParaDTO GetClass(StudentConsolidateCertificateGetClassParaDTO data);
        StudentConsolidateCertificateGetClassParaDTO GetSectionDetails(StudentConsolidateCertificateGetClassParaDTO data);
        StudentConsolidateCertificateGetClassParaDTO GetCertificateDetails(StudentConsolidateCertificateGetClassParaDTO data);
        StudentConsolidateCertificateGetClassParaDTO GetStudentDetails(StudentConsolidateCertificateGetClassParaDTO data);
    }
}

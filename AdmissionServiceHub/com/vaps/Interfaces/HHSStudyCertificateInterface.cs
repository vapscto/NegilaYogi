using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface HHSStudyCertificateInterface
    {
        HHSStudyCertificateDTO getdetails(HHSStudyCertificateDTO data);
        HHSStudyCertificateDTO getstudlist(HHSStudyCertificateDTO stu);
        Task<HHSStudyCertificateDTO> getStudDetails(HHSStudyCertificateDTO studData);
        Task<HHSStudyCertificateDTO> MigrationCertificateStuddetails(HHSStudyCertificateDTO studData);
        HHSStudyCertificateDTO onacademicyearchange(HHSStudyCertificateDTO data);
        HHSStudyCertificateDTO searchfilter(HHSStudyCertificateDTO data);
        HHSStudyCertificateDTO getstudentname(HHSStudyCertificateDTO data);

        //Certificate Generated ReportS
        HHSStudyCertificateDTO CertificateGeneratedReportLoad(HHSStudyCertificateDTO data);
        HHSStudyCertificateDTO GetCertificateGeneratedReport(HHSStudyCertificateDTO data);
        
    }
}

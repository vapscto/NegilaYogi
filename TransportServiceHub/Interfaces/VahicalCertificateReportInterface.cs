using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface VahicalCertificateReportInterface
    {
        VahicalCertificateReportDTO getdata(int id);
        VahicalCertificateReportDTO savedata(VahicalCertificateReportDTO data);
        VahicalCertificateReportDTO edit(VahicalCertificateReportDTO data);
        VahicalCertificateReportDTO Onvahiclechange(VahicalCertificateReportDTO data);
        VahicalCertificateReportDTO deleterecord(VahicalCertificateReportDTO data);

        


    }
}

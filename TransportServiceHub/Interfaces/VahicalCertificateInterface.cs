using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface VahicalCertificateInterface
    {
        VahicalCertificateDTO getdata(int id);
        VahicalCertificateDTO savedata(VahicalCertificateDTO data);
        VahicalCertificateDTO edit(VahicalCertificateDTO data);
        VahicalCertificateDTO Onvahiclechange(VahicalCertificateDTO data);
        VahicalCertificateDTO deleterecord(VahicalCertificateDTO data);

        


    }
}

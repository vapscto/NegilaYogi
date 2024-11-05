using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface SRKVSStudyCertificateInterface
    {
        SRKVSStudycertificateDTO getdetails(SRKVSStudycertificateDTO stu);
        SRKVSStudycertificateDTO getstudlist(SRKVSStudycertificateDTO stu);
        Task<SRKVSStudycertificateDTO> getStudDetails(SRKVSStudycertificateDTO studData);
        SRKVSStudycertificateDTO searchfilter(SRKVSStudycertificateDTO data);
        SRKVSStudycertificateDTO Studdetailsconduct(SRKVSStudycertificateDTO data);
        SRKVSStudycertificateDTO onacademicyearchange(SRKVSStudycertificateDTO data);

    }
}

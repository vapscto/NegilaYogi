using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface JSHSAdmissionCertificateInterface
    {
        JSHSAdmissionCertificate_DTO getdata(JSHSAdmissionCertificate_DTO data);
        JSHSAdmissionCertificate_DTO searchfilter(JSHSAdmissionCertificate_DTO data);
        JSHSAdmissionCertificate_DTO onchangeyear(JSHSAdmissionCertificate_DTO data);
        JSHSAdmissionCertificate_DTO onchangeclass(JSHSAdmissionCertificate_DTO data);
        JSHSAdmissionCertificate_DTO onchangesection(JSHSAdmissionCertificate_DTO data);
        Task<JSHSAdmissionCertificate_DTO> getStudData(JSHSAdmissionCertificate_DTO stuDTO);
    }
}

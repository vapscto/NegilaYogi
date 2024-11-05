using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface TransferCertificateInterface
    {
        TransferCertificate_DTO getdetails(TransferCertificate_DTO sddto);
        Task<TransferCertificate_DTO> tcApply(TransferCertificate_DTO sddto);
        TransferCertificate_DTO deactiveY(TransferCertificate_DTO data);
        Task<TransferCertificate_DTO> certificateApproved(TransferCertificate_DTO sddto);
        Task<TransferCertificate_DTO> certificateRejected(TransferCertificate_DTO data);
        TransferCertificate_DTO CheckApproved_ststus(TransferCertificate_DTO data);
        TransferCertificate_DTO savedetails_certificate(TransferCertificate_DTO data);
        TransferCertificate_DTO edit_certificate(TransferCertificate_DTO data);
        TransferCertificate_DTO deactive_certificate(TransferCertificate_DTO data);
        TransferCertificate_DTO editdata(TransferCertificate_DTO data);

    }
}

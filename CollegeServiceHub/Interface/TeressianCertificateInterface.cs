using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface TeressianCertificateInterface
    {
        TeressianCertificateDTO getalldetails(TeressianCertificateDTO data);
        TeressianCertificateDTO getcoursedata(TeressianCertificateDTO data);
        TeressianCertificateDTO getbranchdata(TeressianCertificateDTO data);
        TeressianCertificateDTO getsemisterdata(TeressianCertificateDTO data);
        TeressianCertificateDTO getsstudentdata(TeressianCertificateDTO data);
        TeressianCertificateDTO GetCertificate(TeressianCertificateDTO data);      
    }
}

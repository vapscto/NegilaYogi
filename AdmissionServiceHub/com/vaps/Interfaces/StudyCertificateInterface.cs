using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudyCertificateInterface
    {
        StudycertificateDTO getdetails(StudycertificateDTO stu);
        StudycertificateDTO getstudlist(StudycertificateDTO stu);
        Task<StudycertificateDTO> getStudDetails(StudycertificateDTO studData);
        StudycertificateDTO onacademicyearchange(StudycertificateDTO data);
        StudycertificateDTO searchfilter(StudycertificateDTO data);
        StudycertificateDTO Studdetailsconduct(StudycertificateDTO data);
        StudycertificateDTO searchfilterSRKVS(StudycertificateDTO data);

        Task<StudycertificateDTO> getStudDetailsSRKVS(StudycertificateDTO studData);
        

    }
}

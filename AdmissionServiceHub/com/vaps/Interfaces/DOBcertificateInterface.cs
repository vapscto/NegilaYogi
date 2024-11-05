using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface DOBcertificateInterface
    {
        Adm_M_StudentDTO getdetails(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO getStudDatabyclass(Adm_M_StudentDTO data);
        //Adm_M_StudentDTO getStudDetails(Adm_M_StudentDTO studData);
        Task<Adm_M_StudentDTO> getStudDetails(Adm_M_StudentDTO studData);
    }
}

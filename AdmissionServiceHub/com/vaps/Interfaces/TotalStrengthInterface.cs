using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface TotalStrengthInterface
    {
        Adm_M_StudentDTO getdetails(Adm_M_StudentDTO stud);
        Adm_M_StudentDTO getsection(Adm_M_StudentDTO studData);
        Adm_M_StudentDTO getclass(Adm_M_StudentDTO studData);
        Adm_M_StudentDTO getelective(Adm_M_StudentDTO studData);
        
        Task<Adm_M_StudentDTO> getStudDetails(Adm_M_StudentDTO studData);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;


namespace WebApplication1.Interfaces
{
    public interface TransfrPreAdmtoAdmInterface
    {
        Adm_M_StudentDTO getAcademicdata(int id);
        Adm_M_StudentDTO getserdata(Adm_M_StudentDTO data);
       // Adm_M_StudentDTO expoadmi(Adm_M_StudentDTO data);.

          Task<Adm_M_StudentDTO> expoadmi(Adm_M_StudentDTO Adm_M_StudentDTO);
    }
}

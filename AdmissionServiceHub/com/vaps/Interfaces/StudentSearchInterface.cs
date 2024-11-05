using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface StudentSearchInterface
    {
       Adm_M_StudentDTO getData(Adm_M_StudentDTO admDto);
    }
}

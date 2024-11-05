using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface SmartcarddetailsInterface
    {
        Task<Adm_M_StudentDTO> getInitailData(int mi_id);
        Task<Adm_M_StudentDTO> getserdata(Adm_M_StudentDTO data);
        Task<Adm_M_StudentDTO> getstudentdetails(Adm_M_StudentDTO data);
        
    }
}

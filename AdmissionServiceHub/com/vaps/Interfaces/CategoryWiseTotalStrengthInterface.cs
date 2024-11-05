using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface CategoryWiseTotalStrengthInterface
    {
        CategoryWiseTotalStrengthDTO getdetails(CategoryWiseTotalStrengthDTO data);
        Task<CategoryWiseTotalStrengthDTO> Getreportdetails(CategoryWiseTotalStrengthDTO data);
    }
}

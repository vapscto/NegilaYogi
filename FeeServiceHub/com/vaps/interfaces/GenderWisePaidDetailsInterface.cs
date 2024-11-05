using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface GenderWisePaidDetailsInterface
    {

        GenderWisePaidDetailsDTO getdata123(GenderWisePaidDetailsDTO data);

        GenderWisePaidDetailsDTO getsection(GenderWisePaidDetailsDTO data);
        GenderWisePaidDetailsDTO getstudent(GenderWisePaidDetailsDTO data);
        GenderWisePaidDetailsDTO getstuddet(GenderWisePaidDetailsDTO data);
        Task<GenderWisePaidDetailsDTO> getreport(GenderWisePaidDetailsDTO data);

        GenderWisePaidDetailsDTO get_groups(GenderWisePaidDetailsDTO data);
    }
}

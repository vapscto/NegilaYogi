using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface StatusInterface
    {
        Task<CommonDTO> getInitailData(int id);
         CommonDTO getdataonsearchfilter(CommonDTO cdto);
        CommonDTO saveData(CommonDTO cdto); 
    }
}

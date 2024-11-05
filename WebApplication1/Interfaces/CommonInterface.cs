using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface CommonInterface 
    {
        Task<CommonDTO> getPagePreviledges(CommonDTO cdto);
    }
}

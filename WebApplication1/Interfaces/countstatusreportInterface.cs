using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface countstatusreportInterface
    {


        Task<CommonDTO> getInitailData(int id);
        //Task<CommonDTO> getdataonsearchfilter(CommonDTO cdto);
        //Task<CommonDTO> saveData(CommonDTO cdto);
    }
}

using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
   public interface NAAC_MC_443_BandWidth_RangeInterface
    {

        NAAC_MC_443_BandWidth_Range_DTO loaddata(NAAC_MC_443_BandWidth_Range_DTO data);
        NAAC_MC_443_BandWidth_Range_DTO save(NAAC_MC_443_BandWidth_Range_DTO data);
   
        NAAC_MC_443_BandWidth_Range_DTO EditData(NAAC_MC_443_BandWidth_Range_DTO data);

    }
}

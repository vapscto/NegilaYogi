using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
   public interface NAAC_MC_436_EContentInterface
    {
        NAAC_MC_436_EContent_DTO loaddata(NAAC_MC_436_EContent_DTO data);
        NAAC_MC_436_EContent_DTO savedata(NAAC_MC_436_EContent_DTO data);
        NAAC_MC_436_EContent_DTO editdata(NAAC_MC_436_EContent_DTO data);
        NAAC_MC_436_EContent_DTO deactiveStudent(NAAC_MC_436_EContent_DTO data);
        NAAC_MC_436_EContent_DTO viewuploadflies(NAAC_MC_436_EContent_DTO data);
        NAAC_MC_436_EContent_DTO deleteuploadfile(NAAC_MC_436_EContent_DTO data);
    }
}

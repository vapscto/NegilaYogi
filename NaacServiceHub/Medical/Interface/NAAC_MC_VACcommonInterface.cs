using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
   public interface NAAC_MC_VACcommonInterface
    {
       
        NAAC_MC_VACcommon_DTO loaddata(NAAC_MC_VACcommon_DTO data);
        NAAC_MC_VACcommon_DTO savedata141(NAAC_MC_VACcommon_DTO data);
        NAAC_MC_VACcommon_DTO savedata142(NAAC_MC_VACcommon_DTO data);
        NAAC_MC_VACcommon_DTO editdata141(NAAC_MC_VACcommon_DTO data);
        NAAC_MC_VACcommon_DTO M_savedata221(NAAC_MC_VACcommon_DTO data);
        NAAC_MC_VACcommon_DTO M_savedata232(NAAC_MC_VACcommon_DTO data);
        NAAC_MC_VACcommon_DTO M_savedata254(NAAC_MC_VACcommon_DTO data);
       
    }
}

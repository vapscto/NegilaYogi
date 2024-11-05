using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
    public interface HSU_MasterCR2Interface
    {
        HSU_MasterCR2_DTO loaddata(HSU_MasterCR2_DTO data);
        HSU_MasterCR2_DTO save_HSU_221(HSU_MasterCR2_DTO data);
        HSU_MasterCR2_DTO save_HSU_232(HSU_MasterCR2_DTO data);
        HSU_MasterCR2_DTO save_HSU_255(HSU_MasterCR2_DTO data);
    }
}

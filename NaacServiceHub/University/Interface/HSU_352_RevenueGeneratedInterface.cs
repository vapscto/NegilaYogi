using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
   public interface HSU_352_RevenueGeneratedInterface
    {
        HSU_352_RevenueGeneratedDTO loaddata(HSU_352_RevenueGeneratedDTO data);
        HSU_352_RevenueGeneratedDTO save(HSU_352_RevenueGeneratedDTO data);
        HSU_352_RevenueGeneratedDTO deactive(HSU_352_RevenueGeneratedDTO data);
        HSU_352_RevenueGeneratedDTO EditData(HSU_352_RevenueGeneratedDTO data);
        HSU_352_RevenueGeneratedDTO viewuploadflies(HSU_352_RevenueGeneratedDTO data);
        HSU_352_RevenueGeneratedDTO deleteuploadfile(HSU_352_RevenueGeneratedDTO data);
    }
}

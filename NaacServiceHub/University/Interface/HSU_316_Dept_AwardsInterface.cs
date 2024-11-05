using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
   public interface HSU_316_Dept_AwardsInterface
    {
        HSU_316_Dept_AwardsDTO loaddata(HSU_316_Dept_AwardsDTO data);
        HSU_316_Dept_AwardsDTO save(HSU_316_Dept_AwardsDTO data);
        HSU_316_Dept_AwardsDTO deactive(HSU_316_Dept_AwardsDTO data);
        HSU_316_Dept_AwardsDTO EditData(HSU_316_Dept_AwardsDTO data);
        HSU_316_Dept_AwardsDTO viewuploadflies(HSU_316_Dept_AwardsDTO data);
        HSU_316_Dept_AwardsDTO deleteuploadfile(HSU_316_Dept_AwardsDTO data);
    }
}

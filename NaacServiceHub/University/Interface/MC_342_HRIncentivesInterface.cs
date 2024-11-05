using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
   public interface MC_342_HRIncentivesInterface
    {
        MC_342_HRIncentivesDTO loaddata(MC_342_HRIncentivesDTO data);
        MC_342_HRIncentivesDTO savedata(MC_342_HRIncentivesDTO data);
        MC_342_HRIncentivesDTO deactive(MC_342_HRIncentivesDTO data);
        MC_342_HRIncentivesDTO editdata(MC_342_HRIncentivesDTO data);
    }
}

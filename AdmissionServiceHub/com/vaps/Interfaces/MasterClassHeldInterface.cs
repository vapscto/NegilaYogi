using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public  interface MasterClassHeldInterface
    {
        MasterClassHeldDTO getAllDetails(MasterClassHeldDTO mch);
        MasterClassHeldDTO saveData(MasterClassHeldDTO mstdto);
        MasterClassHeldDTO getNoOfClassHeld(MasterClassHeldDTO mstdto);
    }
}

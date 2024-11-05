using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;

namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeStreamGroupMappingInterface
    {
        FeeStreamGroupMappingDTO getData(FeeStreamGroupMappingDTO data);

        FeeStreamGroupMappingDTO saveData(FeeStreamGroupMappingDTO data);

        FeeStreamGroupMappingDTO deactivate(FeeStreamGroupMappingDTO id);

        FeeStreamGroupMappingDTO Editdetails(int id);
    }
}

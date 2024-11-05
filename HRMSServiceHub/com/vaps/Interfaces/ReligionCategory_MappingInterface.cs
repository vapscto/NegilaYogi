using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
  public  interface ReligionCategory_MappingInterface
    {
        ReligionCategory_MappingDTO loaddata(ReligionCategory_MappingDTO data);
        //ReligionCategory_MappingDTO getcast(ReligionCategory_MappingDTO data);
        ReligionCategory_MappingDTO savedata(ReligionCategory_MappingDTO data);
        ReligionCategory_MappingDTO Editdata(ReligionCategory_MappingDTO data);
        ReligionCategory_MappingDTO masterDecative(ReligionCategory_MappingDTO data);
    }
}

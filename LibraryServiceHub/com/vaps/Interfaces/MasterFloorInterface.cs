using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterFloorInterface
    {
        MasterFloorDTO Savedata(MasterFloorDTO data);
        MasterFloorDTO getdetails(int id);
        MasterFloorDTO deactiveY(MasterFloorDTO data);
    }
}

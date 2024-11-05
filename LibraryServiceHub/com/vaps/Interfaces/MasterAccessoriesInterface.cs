using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterAccessoriesInterface
    {

        LIB_Master_Accessories_DTO Savedata(LIB_Master_Accessories_DTO data);
        LIB_Master_Accessories_DTO getdetails(int id);
        LIB_Master_Accessories_DTO deactiveY(LIB_Master_Accessories_DTO data);

    }
}

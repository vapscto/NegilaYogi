using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterAuthorInterface
    {
        LIB_Master_Author_DTO Savedata(LIB_Master_Author_DTO data);
        LIB_Master_Author_DTO getdetails(LIB_Master_Author_DTO id);
        LIB_Master_Author_DTO deactiveY(LIB_Master_Author_DTO data);
    }
}

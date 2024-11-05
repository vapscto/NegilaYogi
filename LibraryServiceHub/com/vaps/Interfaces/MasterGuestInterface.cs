using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterGuestInterface
    {
        MasterGuestDTO Savedata(MasterGuestDTO data);
        MasterGuestDTO getdetails(int id);
        MasterGuestDTO deactiveY(MasterGuestDTO data);
    }
}

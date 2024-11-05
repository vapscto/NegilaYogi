using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterVendorInterface
    {
        MasterVendorDTO Savedata(MasterVendorDTO data);
        MasterVendorDTO getdetails(int id);
        MasterVendorDTO deactiveY(MasterVendorDTO data);
    }
}

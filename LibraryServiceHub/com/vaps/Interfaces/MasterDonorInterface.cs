using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterDonorInterface
    {
        MasterDonorDTO Savedata(MasterDonorDTO data);
        MasterDonorDTO getdetails(int id);
        MasterDonorDTO deactiveY(MasterDonorDTO data);
    }
}

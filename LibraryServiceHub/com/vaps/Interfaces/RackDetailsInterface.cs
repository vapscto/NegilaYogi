using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface RackDetailsInterface
    {
        RackDetailsDTO getdetails(int id);
        RackDetailsDTO Savedata(RackDetailsDTO data);
        RackDetailsDTO EditData(RackDetailsDTO data);
        RackDetailsDTO deactiveY(RackDetailsDTO data);
    }
}

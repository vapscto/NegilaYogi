using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterTimeSlabInterface
    {
        MasterTimeSlabDTO getdetails(int id);
        MasterTimeSlabDTO Savedata(MasterTimeSlabDTO data);
        MasterTimeSlabDTO deactiveY(MasterTimeSlabDTO data);
    }
}

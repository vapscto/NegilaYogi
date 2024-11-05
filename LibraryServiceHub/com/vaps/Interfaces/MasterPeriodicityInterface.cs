using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterPeriodicityInterface  //Savedata
    {
        MasterPeriodicityDTO Savedata(MasterPeriodicityDTO data);
        MasterPeriodicityDTO getdetails(int id);
        MasterPeriodicityDTO deactiveY(MasterPeriodicityDTO data);
    }
}

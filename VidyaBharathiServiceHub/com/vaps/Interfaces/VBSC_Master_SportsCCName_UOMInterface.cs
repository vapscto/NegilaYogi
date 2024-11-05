using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
   public interface VBSC_Master_SportsCCName_UOMInterface
    {
        VBSC_Master_SportsCCName_UOMDTO getDetails(VBSC_Master_SportsCCName_UOMDTO data);
        VBSC_Master_SportsCCName_UOMDTO saveRecord(VBSC_Master_SportsCCName_UOMDTO obj);
        VBSC_Master_SportsCCName_UOMDTO EditDetails(int id);
        VBSC_Master_SportsCCName_UOMDTO deactivate(VBSC_Master_SportsCCName_UOMDTO obj);
    }
}


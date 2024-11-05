using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
    public interface VBSC_Master_UOMInterface
    {
        VBSC_Master_UOMDTO loaddata(int dto);
        VBSC_Master_UOMDTO savedetails(VBSC_Master_UOMDTO data);
        VBSC_Master_UOMDTO deactive(VBSC_Master_UOMDTO data);

        //competition level
        VBSC_Master_UOMDTO getloaddatalevel(int dto);
        VBSC_Master_UOMDTO savedetailslevel(VBSC_Master_UOMDTO data);
        VBSC_Master_UOMDTO deactivelevel(VBSC_Master_UOMDTO data);
        
    }
}

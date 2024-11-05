using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.Transport;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface AreaGroupMappingInterface
    {
        AreaGroupMappingDTO getdetails(int id);
        AreaGroupMappingDTO getpageedit(int id);
        AreaGroupMappingDTO Savedetails(AreaGroupMappingDTO org);
        AreaGroupMappingDTO deactivate(AreaGroupMappingDTO id);

        TR_Area_AmountDTO savedataamount(TR_Area_AmountDTO data);
        TR_Area_AmountDTO geteditdataamount(TR_Area_AmountDTO data);
        TR_Area_AmountDTO activedeactiveamount(TR_Area_AmountDTO data);
    }
}

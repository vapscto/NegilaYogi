using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public  interface LocationFeeGroupMappingInterface
    {
        TR_Location_FeeGroup_MappingDTO getdata(int id);
        TR_Location_FeeGroup_MappingDTO savedata(TR_Location_FeeGroup_MappingDTO data);
        TR_Location_FeeGroup_MappingDTO geteditdata(TR_Location_FeeGroup_MappingDTO data);
        TR_Location_FeeGroup_MappingDTO activedeactive(TR_Location_FeeGroup_MappingDTO data);

        TR_Location_AmountDTO savedataamount(TR_Location_AmountDTO data);
        TR_Location_AmountDTO geteditdataamount(TR_Location_AmountDTO data);
        TR_Location_AmountDTO activedeactiveamount(TR_Location_AmountDTO data);
    }
}

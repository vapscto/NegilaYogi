using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface RoomMappingInterface
    {
        RoomMappingDTO getdetails(RoomMappingDTO data);
        RoomMappingDTO get_catg(RoomMappingDTO objcategory);
        RoomMappingDTO deactiveY(RoomMappingDTO objcategory);
        RoomMappingDTO getreport(RoomMappingDTO objcategory);
        RoomMappingDTO getpossiblePeriod(RoomMappingDTO objcategory);
        RoomMappingDTO savedetail(RoomMappingDTO objperiod);
        RoomMappingDTO editdata(RoomMappingDTO objperiod);
    }
}

using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface CLGRoomMappingInterface
    {
        CLGRoomMappingDTO getdetails(CLGRoomMappingDTO data);
        CLGRoomMappingDTO get_catg(CLGRoomMappingDTO objcategory);
        CLGRoomMappingDTO deactiveY(CLGRoomMappingDTO objcategory);
        CLGRoomMappingDTO getdays(CLGRoomMappingDTO objcategory);
        CLGRoomMappingDTO getpossiblePeriod(CLGRoomMappingDTO objcategory);
        CLGRoomMappingDTO savedetail(CLGRoomMappingDTO objperiod);
        CLGRoomMappingDTO editdata(CLGRoomMappingDTO objperiod);
        CLGRoomMappingDTO get_roomfacility(CLGRoomMappingDTO objperiod);
   
    }
}

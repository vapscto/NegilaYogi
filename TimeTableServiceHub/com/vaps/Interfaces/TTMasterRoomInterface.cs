using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface TTMasterRoomInterface
    {
        TTMasterRoomDTO savedetail(TTMasterRoomDTO objcategory);
        TTMasterRoomDTO getdetails(int id);
        TTMasterRoomDTO Viewfacility(int id);
        TTMasterRoomDTO getpageedit(int id);
        TTMasterRoomDTO deactivate(TTMasterRoomDTO id);
    }
}

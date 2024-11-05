using PreadmissionDTOs.com.vaps.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface RoomWebInterface
    {
        HR_Master_Room_DTO getBasicData(HR_Master_Room_DTO dto);
       
    }
}

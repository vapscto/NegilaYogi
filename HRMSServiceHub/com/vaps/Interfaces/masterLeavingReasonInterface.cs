using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
   public interface masterLeavingReasonInterface
    {
        masterLeavingReasonDTO loaddata(masterLeavingReasonDTO data);
        masterLeavingReasonDTO savedata(masterLeavingReasonDTO data);
        masterLeavingReasonDTO EditData(masterLeavingReasonDTO data);
        masterLeavingReasonDTO masterDecative(masterLeavingReasonDTO data);
    }
}

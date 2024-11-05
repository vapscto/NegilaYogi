using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface OnlineLeaveApplicationInterface
    {
        LeaveCreditDTO getonlineLeave(LeaveCreditDTO data);       
        LeaveCreditDTO saveonlineLeave(LeaveCreditDTO data);
        LeaveCreditDTO saveadminLeave(LeaveCreditDTO data);        
        LeaveCreditDTO getonlineLeavestatus(LeaveCreditDTO data);
        LeaveCreditDTO getemployeeadmin(LeaveCreditDTO data);
        LeaveCreditDTO getSingleEmpLeavestatus(LeaveCreditDTO data);

        LeaveCreditDTO deactivate(LeaveCreditDTO dto);
        LeaveCreditDTO requestleave(LeaveCreditDTO dto);

        //----///////////////////////periodwiseleave///////////////////////////////////////
        LeaveCreditDTO getdetails(LeaveCreditDTO dto);
        LeaveCreditDTO getabsentstaff(LeaveCreditDTO dto);
        LeaveCreditDTO get_free_stfdets(LeaveCreditDTO dto);
        LeaveCreditDTO get_period_alloted(LeaveCreditDTO dto);
        LeaveCreditDTO savedetails(LeaveCreditDTO dto);
        LeaveCreditDTO updatedetails(LeaveCreditDTO dto);
    }
}

using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface StaffMasterInterface
    {
        TTStaffMasterDTO savedetail(TTStaffMasterDTO objcategory);
        TTStaffMasterDTO deactivate(TTStaffMasterDTO data);
        TTStaffMasterDTO getdetails(int id);
        TTStaffMasterDTO getpageedit(int id);
        TTStaffMasterDTO deleterec(int id);


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface MasterLeaveInterface
    {
        MasterLeaveDTO GetLeave(MasterLeaveDTO data);

        MasterLeaveDTO saveData(MasterLeaveDTO data);

        MasterLeaveDTO validateordernumber(MasterLeaveDTO data);

        MasterLeaveDTO Edit(int id);
        MasterLeaveDTO deletepages(MasterLeaveDTO data);

        MasterLeaveDTO deactivate(MasterLeaveDTO data);

        MasterLeaveDTO searchByColumn( MasterLeaveDTO data);
        MasterLeaveDTO getpageedit(int id);


    }
}

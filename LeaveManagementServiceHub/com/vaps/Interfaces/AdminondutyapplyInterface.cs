using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
   public interface AdminondutyapplyInterface
    {
        AdminondutyapplyDTO getdata(AdminondutyapplyDTO data);
        AdminondutyapplyDTO employeedetails(AdminondutyapplyDTO data);
        AdminondutyapplyDTO requestleave(AdminondutyapplyDTO data);
        AdminondutyapplyDTO viewcomment(AdminondutyapplyDTO data);
        AdminondutyapplyDTO ActiveDeactiveRecord(AdminondutyapplyDTO data);
        AdminondutyapplyDTO editData(AdminondutyapplyDTO data);
    }
}

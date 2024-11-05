using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface StaffCompliantsInterface
    {
        StaffCompliantsDTO loaddata(StaffCompliantsDTO data);
        StaffCompliantsDTO OnChangeEmployee(StaffCompliantsDTO data);
        StaffCompliantsDTO SaveDetails(StaffCompliantsDTO data);
        StaffCompliantsDTO EditDetails(StaffCompliantsDTO data);
        StaffCompliantsDTO ActiveDeativeEmployeeCompliantsDetails(StaffCompliantsDTO data);
        StaffCompliantsDTO GetReport(StaffCompliantsDTO data);
        StaffCompliantsDTO GetViewStaffLoaddata(StaffCompliantsDTO data);
    }
}

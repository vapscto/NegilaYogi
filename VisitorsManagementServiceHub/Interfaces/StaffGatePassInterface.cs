using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
   public interface StaffGatePassInterface
    {
        StaffGatePass_DTO Getdetails(StaffGatePass_DTO data);
        StaffGatePass_DTO getdepchange(StaffGatePass_DTO data);
        StaffGatePass_DTO get_staff1(StaffGatePass_DTO data);
        StaffGatePass_DTO saverecord(StaffGatePass_DTO data);
        StaffGatePass_DTO editrecord(StaffGatePass_DTO id);
        StaffGatePass_DTO deactive(StaffGatePass_DTO data);
        StaffGatePass_DTO PrintGatePass(StaffGatePass_DTO data);
    }
}

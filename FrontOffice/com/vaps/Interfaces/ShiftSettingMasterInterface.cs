using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
  public  interface ShiftSettingMasterInterface
    {
        MasterShiftsTimingsDTO getdata(MasterShiftsTimingsDTO data);
        MasterShiftsTimingsDTO savedatadelegate(MasterShiftsTimingsDTO data);
        MasterShiftsTimingsDTO getpageedit(int id);
        MasterShiftsTimingsDTO deactivate(MasterShiftsTimingsDTO id);
        MasterShiftsTimingsDTO getalldetailsviewrecords1(int id);
    }
}

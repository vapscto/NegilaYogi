using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public interface HostelAllotForStaffInterface
    {
        Task<HostelAllotForStaff_DTO> loaddata(HostelAllotForStaff_DTO data);
        HostelAllotForStaff_DTO savedata(HostelAllotForStaff_DTO data);
        Task<HostelAllotForStaff_DTO> get_studInfo(HostelAllotForStaff_DTO data);
        HostelAllotForStaff_DTO get_roomdetails(HostelAllotForStaff_DTO data);
        HostelAllotForStaff_DTO getdesg(HostelAllotForStaff_DTO data);
        HostelAllotForStaff_DTO deactivYTab1(HostelAllotForStaff_DTO data);
        Task<HostelAllotForStaff_DTO> editdata(HostelAllotForStaff_DTO data);
    }
}

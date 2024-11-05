using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public interface HostelAllotForStudentInterface
    {
        Task<HostelAllotForStudent_DTO> loaddata(HostelAllotForStudent_DTO data);
        HostelAllotForStudent_DTO savedata(HostelAllotForStudent_DTO data);
        Task<HostelAllotForStudent_DTO> get_studInfo(HostelAllotForStudent_DTO data);
        HostelAllotForStudent_DTO get_roomdetails(HostelAllotForStudent_DTO data);
        Task<HostelAllotForStudent_DTO> editdata(HostelAllotForStudent_DTO data);
    }
}

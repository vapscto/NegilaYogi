using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
   public interface LateInStudentInterface
    {
        Task<LateInStudent_DTO> loaddata (LateInStudent_DTO data);
        LateInStudent_DTO get_class(LateInStudent_DTO data);
        LateInStudent_DTO get_section(LateInStudent_DTO data);
        LateInStudent_DTO get_student(LateInStudent_DTO data);
        Task<LateInStudent_DTO> savedata(LateInStudent_DTO data);
        LateInStudent_DTO editdata(LateInStudent_DTO data);
        LateInStudent_DTO deactive(LateInStudent_DTO data);

    }
}

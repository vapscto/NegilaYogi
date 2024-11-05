using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Interfaces
{
   public interface StudentCompliantsViewInterface
    {
        Task<StudentCompliantsView_DTO> loaddata(StudentCompliantsView_DTO data);
        Task<StudentCompliantsView_DTO> report1(StudentCompliantsView_DTO data);
    }
}

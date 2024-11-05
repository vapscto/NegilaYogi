using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterDepartmentInterface
    {
        MasterDepartmentDTO Savedata(MasterDepartmentDTO data);
        MasterDepartmentDTO getdetails(int id);
        MasterDepartmentDTO deactiveY(MasterDepartmentDTO data);
    }
}

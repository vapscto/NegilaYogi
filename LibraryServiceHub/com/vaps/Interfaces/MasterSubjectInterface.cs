using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterSubjectInterface
    {
        MasterSubject_DTO Savedata(MasterSubject_DTO data);
        MasterSubject_DTO getdetails  (int id);
        MasterSubject_DTO deactiveY(MasterSubject_DTO data);
    }
}

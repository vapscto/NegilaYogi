
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Library;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterLanguageInterface
    {
        MasterLanguageDTO Savedata(MasterLanguageDTO data); 
        MasterLanguageDTO getdetails(int id);
        MasterLanguageDTO deactiveY(MasterLanguageDTO data); 
    }
}

using System;
using PreadmissionDTOs.com.vaps.Library;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
  public interface MasterCategoryInterface 
    {
        MasterCategory_DTO Savedata(MasterCategory_DTO data);
        MasterCategory_DTO deactiveY(MasterCategory_DTO data);
        MasterCategory_DTO getdetails(int id);
    }
}

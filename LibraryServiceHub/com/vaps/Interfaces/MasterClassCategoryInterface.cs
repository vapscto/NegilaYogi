using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterClassCategoryInterface
    {

        LIB_Master_ClassCategory_DTO Savedata(LIB_Master_ClassCategory_DTO data);
        Task<LIB_Master_ClassCategory_DTO> getdetails(LIB_Master_ClassCategory_DTO id);
        LIB_Master_ClassCategory_DTO deactiveY(LIB_Master_ClassCategory_DTO data);

    }
}

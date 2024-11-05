using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface LostBookInterface
    {

        LostBook_DTO getdetails(LostBook_DTO data);
        LostBook_DTO searchfilter(LostBook_DTO data);
        Task<LostBook_DTO> get_authorNm(LostBook_DTO data);
        Task<LostBook_DTO> get_radiochange(LostBook_DTO data);
        LostBook_DTO saverecord(LostBook_DTO data);
        
    }
}

using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface MasterLibraryInterface
    {

        LIB_Master_Library_DTO Savedata(LIB_Master_Library_DTO data);
        Task<LIB_Master_Library_DTO> getdetails(LIB_Master_Library_DTO id);
        LIB_Master_Library_DTO deactiveY(LIB_Master_Library_DTO data);
        LIB_Master_Library_DTO saveclassdata(LIB_Master_Library_DTO data);
        LIB_Master_Library_DTO deactiveYstf(LIB_Master_Library_DTO data);
        LIB_Master_Library_DTO EditstaffData(LIB_Master_Library_DTO data);
        Task<LIB_Master_Library_DTO> modalclsslst(LIB_Master_Library_DTO data);
        LIB_Master_Library_DTO deactivclsdata(LIB_Master_Library_DTO data);
        Task<LIB_Master_Library_DTO> getusername(LIB_Master_Library_DTO data);
        LIB_Master_Library_DTO check_userclass(LIB_Master_Library_DTO data);

        LIB_Master_Library_DTO EditclassData(LIB_Master_Library_DTO data);
        LIB_Master_Library_DTO get_MappedClasslist(LIB_Master_Library_DTO data);

        LIB_Master_Library_DTO savestaffdata(LIB_Master_Library_DTO data);
        
    }
}
